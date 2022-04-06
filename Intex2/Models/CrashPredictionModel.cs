using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ML.OnnxRuntime.Tensors;

namespace Intex2.Models
{
    public class CrashPredictionModel
    {
        // Put the range from 0 to 1 since it is a dummy variable
        [Range(0, 1)]
        public float PedestrianTrue { get; set; }
        [Range(0, 1)]
        public float BicyclistTrue { get; set; }
        [Range(0, 1)]
        public float MotorcycleTrue { get; set; }
        [Range(0, 1)]
        public float ImproperRestraintTrue { get; set; }
        [Range(0, 1)]
        public float UnrestrainTrue { get; set; }
        [Range(0, 1)]
        public float DuiTrue { get; set; }
        [Range(0, 1)]
        public float IntersectionTrue { get; set; }
        [Range(0, 1)]
        public float WildAnimalTrue { get; set; }
        [Range(0, 1)]
        public float DomesticAnimalTrue { get; set; }
        [Range(0, 1)]
        public float OverturnRolloverTrue { get; set; }
        [Range(0, 1)]
        public float CommercialTrue { get; set; }
        [Range(0, 1)]
        public float TeenageDriverTrue { get; set; }
        [Range(0, 1)]
        public float OlderDriverTrue { get; set; }
        [Range(0, 1)]
        public float NightDarkTrue { get; set; }
        [Range(0, 1)]
        public float SingleVehicleTrue { get; set; }
        [Range(0, 1)]
        public float DistractedTrue { get; set; }
        [Range(0, 1)]
        public float DrowsyDrivingTrue { get; set; }
        [Range(0, 1)]
        public float RoadwayDepartureTrue { get; set; }

        public Tensor<float> AsTensor()
        {
            float[] data = new float[]
            {
                    PedestrianTrue, BicyclistTrue, MotorcycleTrue, ImproperRestraintTrue,
                    UnrestrainTrue, DuiTrue, IntersectionTrue, WildAnimalTrue, DomesticAnimalTrue,
                    OverturnRolloverTrue, CommercialTrue, TeenageDriverTrue, OlderDriverTrue,
                    NightDarkTrue, SingleVehicleTrue, DistractedTrue, DrowsyDrivingTrue,
                    RoadwayDepartureTrue
            };

            // 18 features
            int[] dimensions = new int[] { 1, 18 };
            return new DenseTensor<float>(data, dimensions);
        }
    }
}


