using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Shared.Models.PredictionData
{
    public class PredictionDataModel
    {
        public class RepairData
        {
            public float Mileage { get; set; }
            public float Cost { get; set; }
            public string RepairTypeId { get; set; } // <--- Делаем СТРОКОЙ!
            public float Label { get; set; }
        }

        public class RepairPrediction
        {
            [ColumnName("Score")]
            public float PredictedRemainingMileage { get; set; }
        }
    }
}
