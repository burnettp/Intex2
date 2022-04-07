using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Intex2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;

namespace Intex2.Controllers
{
    [Authorize(Roles = "Admin, Reader")]
    public class InferenceController : Controller
    {
        private InferenceSession _session;

        public InferenceController(InferenceSession session)
        {
            _session = session;
        }

        // Pull up the Score page
        [HttpGet]
        public IActionResult Score()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Score(CrashPredictionModel data)
        {
            var result = _session.Run(new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor("float_input", data.AsTensor())
            });
            Tensor<float> score = result.First().AsTensor<float>();
            var prediction = new Prediction { PredictedValue = score.First() };
            result.Dispose();

            // Set the prediction equal to a viewbag so we can use it in the view
            ViewBag.Pred = prediction.PredictedValue;

            // Return the view
            return View();
        }
    }
}

