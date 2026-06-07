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
            [LoadColumn(0)] public float Mileage { get; set; }     // Пробег
            [LoadColumn(1)] public float Cost { get; set; }        // Стоимость ремонта
            [LoadColumn(2)] public float Label { get; set; }       // То, что предсказываем (например, остаток ресурса)
        }

        // То, что нейросеть нам вернет
        public class RepairPrediction
        {
            [ColumnName("Score")]
            public float PredictedRemainingMileage { get; set; }
        }
    }
}
