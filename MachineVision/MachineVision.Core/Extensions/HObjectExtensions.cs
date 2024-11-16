using HalconDotNet;
using MachineVision.Core.TemplateMatch;
using System.Xaml;

namespace MachineVision.Core.Extensions
{
    public static class HObjectExtensions
    {
        public static HObject ReduceDomain(this HObject image, HObject region)
        {
            HOperatorSet.ReduceDomain(image, region, out HObject template);
            return template;
        }

        public static HObject CropDomain(this HObject image)
        {
            HOperatorSet.CropDomain(image, out HObject template);
            return template;
        }

        public static HObject ReduceDomain(this HObject image, double x1, double y1, double x2, double y2)
        {
            HOperatorSet.GenRectangle1(out HObject rectangle, y1, x1, y2, x2);
            HOperatorSet.ReduceDomain(image, rectangle, out HObject template);
            return template;
        }

        public static HObject ReduceDomain(this HObject image, RoiParameter roi)
        {
            if (roi == null)
                return image;

            if (roi.Row1 == 0 && roi.Column1 == 0 && roi.Row2 == 0 && roi.Column2 == 0)
                return image;

            HOperatorSet.GenRectangle1(out HObject rectangle, roi.Row1, roi.Column1, roi.Row2, roi.Column2);
            HOperatorSet.ReduceDomain(image, rectangle, out HObject imageReduced);
            return imageReduced;
        }

        /// <summary>
        /// 转换灰度图像
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static HObject Rgb1ToGray(this HObject image)
        {
            HOperatorSet.Rgb1ToGray(image, out HObject ho_GrayImage);
            return ho_GrayImage;
        }

        /// <summary>
        /// 获取图像尺寸
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static int[] GetImageSize(this HObject image)
        {
            int width, height;
            HImage img = new HImage();
            HobjectToHimage(image, ref img);
            img.GetImageSize(out width, out height);
            return new int[] { width, height };

            static void HobjectToHimage(HObject hobject, ref HImage image)
            {
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    HTuple p, t, w, h;
                    HOperatorSet.GetImagePointer1(hobject, out p, out t, out w, out h);
                    image.GenImage1(t, w, h, p);
                }
            }
        }

        /// <summary>
        /// 保存BMP图像
        /// </summary>
        /// <param name="image"></param>
        /// <param name="fileName"></param>
        public static void SaveIamge(this HObject image, string fileName)
        {
            HOperatorSet.WriteImage(image, "bmp", 0, fileName);
        }

        /// <summary>
        ///  移动区域
        /// </summary>
        /// <param name="region"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>

        public static HObject Move(this HObject region, double row, double column)
        {
            HObject ho_moveregion;
            HOperatorSet.GenEmptyObj(out ho_moveregion);
            HOperatorSet.MoveRegion(region, out ho_moveregion, row, column);
            return ho_moveregion;
        }

        /// <summary>
        /// 获取区域轮廓
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        public static HObject GetRegionContour(this HObject region)
        {
            HOperatorSet.GenContourRegionXld(region, out HObject contours, "border");
            return contours;
        }

        /// <summary>
        /// 获取面积总结
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        public static long GetSumArea(this HObject region)
        {
            HOperatorSet.AreaCenter(region, out HTuple area, out HTuple row, out HTuple column);

            if (area.Length == 0) return 0;

            long sum_area = 0;
            foreach (HTuple h in area.LArr)
                sum_area += h;

            area.Dispose();
            row.Dispose();
            column.Dispose();

            return sum_area;
        }
    }
}
