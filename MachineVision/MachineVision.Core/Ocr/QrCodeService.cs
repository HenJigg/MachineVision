using HalconDotNet;
using MachineVision.Core.Extensions;
using MachineVision.Core.TemplateMatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineVision.Core.Ocr
{
    public class QrCodeService
    {
        public QrCodeService()
        {
            HOperatorSet.CreateDataCode2dModel("QR Code", new HTuple(), new HTuple(), out hv_DataCodeHandle);

            Info = new MethodInfo()
            {
                Name = "find_data_code_2d",
                Description = "Detect and read 2D data code symbols in an image or train the 2D data code model.",
                Parameters = new List<MathodParameter>()
                {
                   new MathodParameter(){ Name="Image", Description="Input image. If the image has a reduced domain, the datacode search is reduced to that domain. This usually reduces the runtime of the operator. However, if the datacode is not fully inside the domain, the datacode might not be found correctly." },
                   new MathodParameter(){ Name="SymbolXLDs", Description="XLD contours that surround the successfully decoded data code symbols." },
                   new MathodParameter(){ Name="DataCodeHandle", Description="Handle of the 2D data code model." },
                   new MathodParameter(){ Name="GenParamNames", Description="Names of (optional) parameters for controlling the behavior of the operator." },
                   new MathodParameter(){ Name="GenParamValues", Description="Values of the optional generic parameters." },
                   new MathodParameter(){ Name="ResultHandles", Description="Handles of all successfully decoded 2D data code symbols." },
                   new MathodParameter(){ Name="DecodedDataStrings", Description="Decoded data strings of all detected 2D data code symbols in the image." },
                },
                Predecessors = new List<string>()
                {
                     "create_data_code_2d_model",
                     "read_data_code_2d_model",
                     "set_data_code_2d_param",
                }
            };
        }

        HObject ho_SymbolXLDs = null;
        HTuple hv_ImageFiles = new HTuple(), hv_ImageNum = new HTuple();
        HTuple hv_Message = new HTuple();
        HTuple hv_DataCodeHandle = new HTuple(), hv_Index = new HTuple();
        HTuple hv_ResultHandles = new HTuple(), hv_DecodedDataStrings = new HTuple();

        public RoiParameter Roi { get; set; }
        public MethodInfo Info { get; set; }

        public OcrMatchResult Run(HObject Image)
        {
            var timeSpan = SetTimeHelper.SetTimer(() =>
            {
                HOperatorSet.FindDataCode2d(Image, out ho_SymbolXLDs, hv_DataCodeHandle,
              new HTuple(), new HTuple(), out hv_ResultHandles, out hv_DecodedDataStrings);
            });

            if (!string.IsNullOrWhiteSpace(hv_DecodedDataStrings))
            {
                return new OcrMatchResult()
                {
                    IsSuccess = true,
                    Message = $"{DateTime.Now}: 匹配耗时:{timeSpan} ms ，匹配结果:{hv_DecodedDataStrings}",
                    TimeSpan = timeSpan
                };
            }
            else
            {
                return new OcrMatchResult() { IsSuccess = false, TimeSpan = timeSpan };
            }
        }
    }
}
