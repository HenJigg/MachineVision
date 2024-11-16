using HalconDotNet;
using MachineVision.Core.Extensions;
using MachineVision.Core.TemplateMatch.ShapeModel;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MachineVision.Core.TemplateMatch
{
    /// <summary>
    /// 形状匹配服务
    /// </summary>
    public class ShapeModelService : BindableBase, ITemplateMatchService
    {
        public ShapeModelService()
        {
            Info = new MethodInfo()
            {
                Name = "find_shape_model",
                Description = "Find the best matches of a shape model in an image.",
                Parameters = new List<MathodParameter>()
                {
                   new MathodParameter(){ Name="Image", Description="Input image in which the model should be found." },
                   new MathodParameter(){ Name="ModelID", Description="Handle of the model." },
                   new MathodParameter(){ Name="AngleStart", Description="Smallest rotation of the model." },
                   new MathodParameter(){ Name="AngleExtent", Description="Extent of the rotation angles." },
                   new MathodParameter(){ Name="MinScore", Description="Minimum score of the instances of the model to be found." },
                   new MathodParameter(){ Name="NumMatches", Description="Number of instances of the model to be found (or 0 for all matches)." },
                   new MathodParameter(){ Name="MaxOverlap ", Description="Maximum overlap of the instances of the model to be found." },
                   new MathodParameter(){ Name="SubPixel", Description="Subpixel accuracy if not equal to 'none'." },
                   new MathodParameter(){ Name="NumLevels", Description="Number of pyramid levels used in the matching (and lowest pyramid level to use if |NumLevels| = 2)." },
                   new MathodParameter(){ Name="Greediness", Description="“Greediness” of the search heuristic (0: safe but slow; 1: fast but matches may be missed)." },
                   new MathodParameter(){ Name="Row", Description="Row coordinate of the found instances of the model." },
                   new MathodParameter(){ Name="Column", Description="Column coordinate of the found instances of the model." },
                   new MathodParameter(){ Name="Angle", Description="Rotation angle of the found instances of the model." },
                   new MathodParameter(){ Name="Score", Description="Score of the found instances of the model." },
                },
                Predecessors = new List<string>()
                {
                     "create_shape_model",
                     "read_shape_model",
                     "write_shape_model",
                }
            };
            Roi = new RoiParameter();
            Setting = new MatchResultSetting();

            MatchResult = new MatchResult();
            //初始化模板参数和运行参数的默认值
            RunParameter = new ShapeModelRunParameter();
            TemplateParameter = new ShapeModelInputParameter();
            RunParameter.ApplyDefaultParameter();
            TemplateParameter.ApplyDefaultParameter();
        }

        private HWindow hWindow;

        public HWindow HWindow
        {
            get { return hWindow; }
            set { hWindow = value; RaisePropertyChanged(); }
        }

        private HTuple modelId;
        HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
        HTuple hv_Angle = new HTuple(), hv_Score = new HTuple();

        public RoiParameter Roi { get; set; }
        public MethodInfo Info { get; set; }

        private MatchResultSetting setting;
        private ShapeModelInputParameter templateParameter;
        private ShapeModelRunParameter runParameter;

        /// <summary>
        /// 匹配结果显示设置
        /// </summary>
        public MatchResultSetting Setting
        {
            get { return setting; }
            set { setting = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 模板参数
        /// </summary>
        public ShapeModelInputParameter TemplateParameter
        {
            get { return templateParameter; }
            set { templateParameter = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 运行参数
        /// </summary>
        public ShapeModelRunParameter RunParameter
        {
            get { return runParameter; }
            set { runParameter = value; RaisePropertyChanged(); }
        }

        private MatchResult matchResult;

        public MatchResult MatchResult
        {
            get { return matchResult; }
            set { matchResult = value; RaisePropertyChanged(); }
        }

        public async Task CreateTemplate(HObject image, HObject hObject)
        {
            await Task.Run(() =>
            {
                var template = image.ReduceDomain(hObject).CropDomain();

                HOperatorSet.CreateShapeModel(template,
                   TemplateParameter.NumLevels,
                   TemplateParameter.AngleStart,
                   TemplateParameter.AngleExtent,
                   TemplateParameter.AngleStep,
                   TemplateParameter.Optimization,
                   TemplateParameter.Metric,
                   TemplateParameter.Contrast,
                   TemplateParameter.MinContrast, out modelId);
            });
        }

        public void Run(HObject image)
        {
            MatchResult.Reset();

            if (image == null)
            {
                MatchResult.Message = "输入图像无效";
                return;
            }

            if (modelId == null)
            {
                MatchResult.Message = "输入模板无效";
                return;
            }

            var timeSpan = SetTimeHelper.SetTimer(() =>
             {
                 var imageReduced = image.ReduceDomain(Roi);

                 HOperatorSet.FindShapeModel(
                             imageReduced,
                             modelId,
                             RunParameter.AngleStart,
                             RunParameter.AngleExtent,
                             RunParameter.MinScore,
                             RunParameter.NumMatches,
                             RunParameter.MaxOverlap,
                             RunParameter.SubPixel,
                             RunParameter.NumLevels,
                             RunParameter.Greediness,
                             out hv_Row, out hv_Column, out hv_Angle, out hv_Score);
             });

            //获取形状模板轮廓
            HOperatorSet.GetShapeModelContours(out HObject modelContours, modelId, 1);

            for (int i = 0; i < hv_Score.Length; i++)
            {
                //计算轮廓匹配的目标位置对象
                HOperatorSet.VectorAngleToRigid(0, 0, 0, hv_Row.DArr[i], hv_Column.DArr[i], hv_Angle.DArr[i], out HTuple homMat2D);
                HOperatorSet.AffineTransContourXld(modelContours, out HObject contoursAffineTrans, homMat2D);

                MatchResult.Results.Add(new TemplateMatchResult()
                {
                    Index = i + 1,
                    Row = hv_Row.DArr[i],
                    Column = hv_Column.DArr[i],
                    Angle = hv_Angle.DArr[i],
                    Score = hv_Score.DArr[i],
                    Contours = contoursAffineTrans
                });
            }

            //在窗口中渲染结果
            if (MatchResult.Results != null)
            {
                foreach (var item in MatchResult.Results)
                {
                    if (Setting.IsShowCenter)
                        HWindow.DispCross(item.Row, item.Column, 30, item.Angle);

                    if (Setting.IsShowDisplayText)
                        HWindow.SetString($"({Math.Round(item.Row, 2)},{Math.Round(item.Column, 2)})", "image", item.Row, item.Column, "black", "true");

                    if (Setting.IsShowMatchRange)
                        HWindow.DispObj(item.Contours);
                }
            }

            MatchResult.TimeSpan = timeSpan;
            MatchResult.Message = $"{DateTime.Now}: 匹配耗时:{timeSpan} ms ，匹配个数:{matchResult.Results.Count}";

            if (MatchResult.Results.Count > 0)
                MatchResult.IsSuccess = true; 
        }
    }
}
