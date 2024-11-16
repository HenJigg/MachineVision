using HalconDotNet;
using MachineVision.Core.Extensions;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MachineVision.Core.TemplateMatch.LocalDeformable
{
    /// <summary>
    /// 局部形变匹配模板服务
    /// </summary>
    public class LocalDeformableService : BindableBase, ITemplateMatchService
    {
        public LocalDeformableService()
        {
            Info = new MethodInfo()
            {
                Name = "find_local_deformable_model",
                Description = "Find the best matches of a local deformable model in an image.",
                Parameters = new List<MathodParameter>()
                {
                   new MathodParameter(){ Name="Image", Description="Input image in which the model should be found." },
                   new MathodParameter(){ Name="ImageRectified", Description="Rectified image of the found model." },
                   new MathodParameter(){ Name="VectorField", Description="Vector field of the rectification transformation." },
                   new MathodParameter(){ Name="DeformedContours", Description="Contours of the found instances of the model." },
                   new MathodParameter(){ Name="ModelID", Description="Handle of the model." },
                   new MathodParameter(){ Name="AngleStart", Description="Smallest rotation of the model." },
                   new MathodParameter(){ Name="AngleExtent", Description="Extent of the rotation angles." },
                   new MathodParameter(){ Name="ScaleRMin", Description="Minimum scale of the model in row direction." },
                   new MathodParameter(){ Name="ScaleRMax", Description="Maximum scale of the model in row direction." },
                   new MathodParameter(){ Name="ScaleCMin", Description="Minimum scale of the model in column direction." },
                   new MathodParameter(){ Name="ScaleCMax", Description="Maximum scale of the model in column direction." },
                   new MathodParameter(){ Name="MinScore", Description="Minumum score of the instances of the model to be found." },
                   new MathodParameter(){ Name="NumMatches", Description="Number of instances of the model to be found (or 0 for all matches)." },
                   new MathodParameter(){ Name="MaxOverlap", Description="Maximum overlap of the instances of the model to be found." },
                   new MathodParameter(){ Name="NumLevels", Description="Number of pyramid levels used in the matching." },
                   new MathodParameter(){ Name="Greediness", Description="“Greediness” of the search heuristic (0: safe but slow; 1: fast but matches may be missed)." },
                   new MathodParameter(){ Name="ResultType", Description="Switch for requested iconic result." },
                   new MathodParameter(){ Name="GenParamName", Description="The general parameter names." },
                   new MathodParameter(){ Name="GenParamValue", Description="Values of the general parameters." },
                },
                Predecessors = new List<string>()
                {
                     "create_local_deformable_model",
                     "create_local_deformable_model_xld",
                     "read_deformable_model",
                }
            };
            Roi = new RoiParameter();
            Setting = new MatchResultSetting();
            MatchResult = new MatchResult();
            //初始化模板参数和运行参数的默认值
            RunParameter = new LocalDeformableRunParameter();
            TemplateParameter = new LocalDeformableInputParameter();
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
        public RoiParameter Roi { get; set; }
        public MatchResultSetting Setting { get; set; }
        public MethodInfo Info { get; set; }
        private LocalDeformableInputParameter templateParameter;
        private LocalDeformableRunParameter runParameter;

        private MatchResult matchResult;

        public MatchResult MatchResult
        {
            get { return matchResult; }
            set { matchResult = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 模板参数
        /// </summary>
        public LocalDeformableInputParameter TemplateParameter
        {
            get { return templateParameter; }
            set { templateParameter = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 运行参数
        /// </summary>
        public LocalDeformableRunParameter RunParameter
        {
            get { return runParameter; }
            set { runParameter = value; RaisePropertyChanged(); }
        }

        public async Task CreateTemplate(HObject image, HObject hObject)
        {
            await Task.Run(() =>
            {
                var template = image.ReduceDomain(hObject).CropDomain();

                HOperatorSet.CreateLocalDeformableModel(template,
                    TemplateParameter.NumLevels,
                    TemplateParameter.AngleStart,
                    TemplateParameter.AngleExtent,
                    TemplateParameter.AngleStep,
                    TemplateParameter.ScaleRmin,
                    TemplateParameter.ScaleRmax,
                    TemplateParameter.ScaleRstep,
                    TemplateParameter.ScaleCmin,
                    TemplateParameter.ScaleCmax,
                    TemplateParameter.ScaleCstep,
                    TemplateParameter.Optimization,
                    TemplateParameter.Metric,
                    TemplateParameter.Contrast,
                    TemplateParameter.MinContrast, new HTuple(), new HTuple(), out modelId);
            });
        }

        public void Run(HObject image)
        {
            MatchResult.Reset();

            if (image == null)
            {
                matchResult.Message = "输入图像无效";
                return;
            }

            if (modelId == null)
            {
                matchResult.Message = "输入模板无效";
                return;
            }

            var imageReduced = image.ReduceDomain(Roi);

            HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
            HTuple hv_Score = new HTuple();

            var timeSpan = SetTimeHelper.SetTimer(() =>
            {
                HOperatorSet.FindLocalDeformableModel(image,
                out RunParameter.ImageRectified,
                out RunParameter.VectorField,
                out RunParameter.DeformedContours,
                modelId,
                RunParameter.AngleStart,
                RunParameter.AngleExtent,
                RunParameter.ScaleRmin,
                RunParameter.ScaleRmax,
                RunParameter.ScaleCmin,
                RunParameter.ScaleCmax,
                RunParameter.MinScore,
                RunParameter.NumMatches,
                RunParameter.MaxOverlap,
                RunParameter.NumLevels,
                RunParameter.Greediness,
                ((new HTuple("image_rectified"))
                .TupleConcat("vector_field"))
                .TupleConcat("deformed_contours"),
                new HTuple(),
                new HTuple(), out hv_Score, out hv_Row, out hv_Column);
            });

            for (int i = 0; i < hv_Score.Length; i++)
            {
                MatchResult.Results.Add(new TemplateMatchResult()
                {
                    Index = i + 1,
                    Row = hv_Row.DArr[i],
                    Column = hv_Column.DArr[i],
                    Score = hv_Score.DArr[i],
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
                }
            }

            MatchResult.TimeSpan = timeSpan;
            MatchResult.Message = $"{DateTime.Now}: 匹配耗时:{timeSpan} ms ，匹配个数:{matchResult.Results.Count}";

            if (MatchResult.Results.Count > 0)
                MatchResult.IsSuccess = true;
        }
    }
}
