using HalconDotNet;
using MachineVision.Core.Extensions;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MachineVision.Core.TemplateMatch.NccModel
{
    /// <summary>
    /// 相关性匹配服务
    /// </summary>
    public class NccModelService : BindableBase, ITemplateMatchService
    {
        public NccModelService()
        {
            Info = new MethodInfo()
            {
                Name = "find_ncc_model",
                Description = "Find the best matches of an NCC model in an image.",
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
                   new MathodParameter(){ Name="Row", Description="Row coordinate of the found instances of the model." },
                   new MathodParameter(){ Name="Column", Description="Column coordinate of the found instances of the model." },
                   new MathodParameter(){ Name="Angle", Description="Rotation angle of the found instances of the model." },
                   new MathodParameter(){ Name="Score", Description="Score of the found instances of the model." },
                },
                Predecessors = new List<string>()
                {
                     "create_ncc_model",
                     "read_ncc_model",
                     "write_ncc_model",
                }
            };
            Roi = new RoiParameter();
            Setting = new MatchResultSetting();
            MatchResult = new MatchResult();
            //初始化模板参数和运行参数的默认值
            RunParameter = new NccModelRunParameter();
            TemplateParameter = new NccModelInputParameter();
            RunParameter.ApplyDefaultParameter();
            TemplateParameter.ApplyDefaultParameter();
        }

        private HWindow hWindow;

        public HWindow HWindow
        {
            get { return hWindow; }
            set { hWindow = value; RaisePropertyChanged(); }
        }

        private NccModelInputParameter templateParameter;
        private NccModelRunParameter runParameter;
        private HTuple modelId;
        HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
        HTuple hv_Angle = new HTuple(), hv_Score = new HTuple();

        private MatchResult matchResult;

        public MatchResult MatchResult
        {
            get { return matchResult; }
            set { matchResult = value; RaisePropertyChanged(); }
        }

        public RoiParameter Roi { get; set; }
        public MatchResultSetting Setting { get; set; }
        public MethodInfo Info { get; set; }

        /// <summary>
        /// 相关性匹配-模板参数
        /// </summary>
        public NccModelInputParameter TemplateParameter
        {
            get { return templateParameter; }
            set { templateParameter = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 相关性匹配-运行参数
        /// </summary>
        public NccModelRunParameter RunParameter
        {
            get { return runParameter; }
            set { runParameter = value; RaisePropertyChanged(); }
        }

        public async Task CreateTemplate(HObject image, HObject hObject)
        {
            await Task.Run(() =>
            {
                /*
                 * 灰度匹配注意,创建模板时，需要将图像的格式转换成灰度图像
                 */
                var template = image.ReduceDomain(hObject).CropDomain();

                HOperatorSet.CreateNccModel(template.Rgb1ToGray(),
                   TemplateParameter.NumLevels,
                   TemplateParameter.AngleStart,
                   TemplateParameter.AngleExtent,
                   TemplateParameter.AngleStep,
                   TemplateParameter.Metric, out modelId);
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

                /*
                 * 灰度匹配注意,匹配结果时，需要将图像的格式转换成灰度图像
                 */

                HOperatorSet.FindNccModel(
                            imageReduced.Rgb1ToGray(),
                            modelId,
                            RunParameter.AngleStart,
                            RunParameter.AngleExtent,
                            RunParameter.MinScore,
                            RunParameter.NumMatches,
                            RunParameter.MaxOverlap,
                            RunParameter.SubPixel,
                            RunParameter.NumLevels,
                            out hv_Row, out hv_Column, out hv_Angle, out hv_Score);
            });

            for (int i = 0; i < hv_Score.Length; i++)
            {
                MatchResult.Results.Add(new TemplateMatchResult()
                {
                    Index = i + 1,
                    Row = hv_Row.DArr[i],
                    Column = hv_Column.DArr[i],
                    Angle = hv_Angle.DArr[i],
                    Score = hv_Score.DArr[i],
                    Contours = GetNccModelContours(modelId, hv_Row.DArr[i], hv_Column.DArr[i], hv_Angle.DArr[i], 0)
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

        /// <summary>
        /// 获取相关性匹配的结果轮廓
        /// </summary>
        /// <param name="hv_ModelID"></param>
        /// <param name="hv_Row"></param>
        /// <param name="hv_Column"></param>
        /// <param name="hv_Angle"></param>
        /// <param name="hv_Model"></param>
        /// <returns></returns>
        public HObject GetNccModelContours(HTuple hv_ModelID, HTuple hv_Row, HTuple hv_Column, HTuple hv_Angle, HTuple hv_Model)
        {
            HObject ho_ModelRegion = null, ho_ModelContours = null;
            HObject ho_ContoursAffinTrans = null, ho_Cross = null;

            HTuple hv_NumMatches = new HTuple(), hv_Index = new HTuple();
            HTuple hv_Match = new HTuple(), hv_HomMat2DIdentity = new HTuple();
            HTuple hv_HomMat2DRotate = new HTuple(), hv_HomMat2DTranslate = new HTuple();
            HTuple hv_RowTrans = new HTuple(), hv_ColTrans = new HTuple();
            HTuple hv_Model_COPY_INP_TMP = new HTuple(hv_Model);

            HOperatorSet.GenEmptyObj(out ho_ModelRegion);
            HOperatorSet.GenEmptyObj(out ho_ModelContours);
            HOperatorSet.GenEmptyObj(out ho_ContoursAffinTrans);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            hv_NumMatches.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_NumMatches = new HTuple(hv_Row.TupleLength());
            }
            if ((int)(new HTuple(hv_NumMatches.TupleGreater(0))) != 0)
            {
                if ((int)(new HTuple((new HTuple(hv_Model_COPY_INP_TMP.TupleLength())).TupleEqual(0))) != 0)
                {
                    hv_Model_COPY_INP_TMP.Dispose();
                    HOperatorSet.TupleGenConst(hv_NumMatches, 0, out hv_Model_COPY_INP_TMP);
                }
                else if ((int)(new HTuple((new HTuple(hv_Model_COPY_INP_TMP.TupleLength())).TupleEqual(1))) != 0)
                {
                    HTuple ExpTmpOutVar_0;
                    HOperatorSet.TupleGenConst(hv_NumMatches, hv_Model_COPY_INP_TMP, out ExpTmpOutVar_0);
                    hv_Model_COPY_INP_TMP.Dispose();
                    hv_Model_COPY_INP_TMP = ExpTmpOutVar_0;
                }
                for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_ModelID.TupleLength())) - 1); hv_Index = (int)hv_Index + 1)
                {
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho_ModelRegion.Dispose();
                        HOperatorSet.GetNccModelRegion(out ho_ModelRegion, hv_ModelID.TupleSelect(
                            hv_Index));
                    }
                    ho_ModelContours.Dispose();
                    HOperatorSet.GenContourRegionXld(ho_ModelRegion, out ho_ModelContours, "border_holes");
                    HTuple end_val13 = hv_NumMatches - 1;
                    HTuple step_val13 = 1;
                    for (hv_Match = 0; hv_Match.Continue(end_val13, step_val13); hv_Match = hv_Match.TupleAdd(step_val13))
                    {
                        if ((int)(new HTuple(hv_Index.TupleEqual(hv_Model_COPY_INP_TMP.TupleSelect(hv_Match)))) != 0)
                        {
                            hv_HomMat2DIdentity.Dispose();
                            HOperatorSet.HomMat2dIdentity(out hv_HomMat2DIdentity);
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_HomMat2DRotate.Dispose();
                                HOperatorSet.HomMat2dRotate(hv_HomMat2DIdentity, hv_Angle.TupleSelect(
                                    hv_Match), 0, 0, out hv_HomMat2DRotate);
                            }
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_HomMat2DTranslate.Dispose();
                                HOperatorSet.HomMat2dTranslate(hv_HomMat2DRotate, hv_Row.TupleSelect(hv_Match),
                                    hv_Column.TupleSelect(hv_Match), out hv_HomMat2DTranslate);
                            }
                            ho_ContoursAffinTrans.Dispose();
                            HOperatorSet.AffineTransContourXld(ho_ModelContours, out ho_ContoursAffinTrans, hv_HomMat2DTranslate);
                        }
                    }
                }
            }
            ho_ModelRegion.Dispose();
            ho_ModelContours.Dispose();

            hv_Model_COPY_INP_TMP.Dispose();
            hv_NumMatches.Dispose();
            hv_Index.Dispose();
            hv_Match.Dispose();
            hv_HomMat2DIdentity.Dispose();
            hv_HomMat2DRotate.Dispose();
            hv_HomMat2DTranslate.Dispose();
            hv_RowTrans.Dispose();
            hv_ColTrans.Dispose();

            return ho_ContoursAffinTrans;
        }
    }
}
