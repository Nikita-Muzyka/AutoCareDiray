using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AutoCareDiray.Shared.Models.PredictionData.PredictionDataModel;

namespace AutoCareDiray.Shared.Service.PredicateDateService
{
    public class PredictionService
    {
        private readonly MLContext _mlContext;

        public PredictionService()
        {
            _mlContext = new MLContext();
        }

        public void TrainModel(IEnumerable<RepairData> data)
        {
            // 1. Превращаем список C# в данные ML.NET
            IDataView trainingData = _mlContext.Data.LoadFromEnumerable(data);

            // 2. Создаем "конвейер" (Pipeline): говорим, что мы предсказываем колонку "Label"
            var pipeline = _mlContext.Transforms.Concatenate("Features", new[] { "Mileage", "Cost" })
                .Append(_mlContext.Regression.Trainers.Sdca(labelColumnName: "Label", maximumNumberOfIterations: 100));

            // 3. Обучаем модель
            var model = pipeline.Fit(trainingData);
        }
    }
}
