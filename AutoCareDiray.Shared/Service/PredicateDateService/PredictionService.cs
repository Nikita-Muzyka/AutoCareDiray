using AutoCareDiray.Shared.Models.PredictionData;
using AutoCareDiray.Shared.Models.RepairModel;
using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using static AutoCareDiray.Shared.Models.PredictionData.PredictionDataModel;

namespace AutoCareDiray.Shared.Service.PredicateDateService
{
    public class PredictionService
    {
        private static readonly MLContext _mlContext = new MLContext();
        private static ITransformer _trainedModel;
        private static bool _isTraining = false;

        public PredictionService() { }

        public void PrepareAndTrain(List<PredictionDataModel.RepairData> trainingData)
        {
            if (_trainedModel != null || _isTraining) return;
            if (trainingData == null || trainingData.Count == 0) return;

            try
            {
                _isTraining = true;
                var dataView = _mlContext.Data.LoadFromEnumerable(trainingData);

                var pipeline = _mlContext.Transforms.Categorical.OneHotEncoding("RepairTypeEncoded", "RepairTypeId")
                    // 1. НОРМАЛИЗУЕМ БОЛЬШИЕ ЧИСЛА (Укрощаем пробег и стоимость)
                    .Append(_mlContext.Transforms.NormalizeMinMax("Mileage"))
                    .Append(_mlContext.Transforms.NormalizeMinMax("Cost"))
                    // 2. СКЛЕИВАЕМ УЖЕ НОРМАЛИЗОВАННЫЕ ДАННЫЕ
                    .Append(_mlContext.Transforms.Concatenate("Features", "Mileage", "Cost", "RepairTypeEncoded"))
                    .Append(_mlContext.Regression.Trainers.Sdca(labelColumnName: "Label", maximumNumberOfIterations: 100));

                _trainedModel = pipeline.Fit(dataView);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"КРИТИЧЕСКАЯ ОШИБКА: {ex.Message}");
            }
            finally
            {
                _isTraining = false;
            }
        }

        // Меняем float repairTypeId на string
        public float PredictNext(float currentMileage, float cost, string repairTypeId)
        {
            if (_trainedModel == null) return 0;

            var predictionEngine = _mlContext.Model.CreatePredictionEngine<PredictionDataModel.RepairData, PredictionDataModel.RepairPrediction>(_trainedModel);

            var input = new PredictionDataModel.RepairData
            {
                Mileage = currentMileage,
                Cost = cost,
                RepairTypeId = repairTypeId // Передаем строку
            };

            var result = predictionEngine.Predict(input);
            return result.PredictedRemainingMileage;
        }
    }
}