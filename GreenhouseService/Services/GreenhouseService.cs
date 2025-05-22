using Domain.Entities;
using Domain.Entities.Actuators;
using Domain.IRepositories;
using Domain.IServices;

namespace GreenhouseService.Services
{
    public class GreenhouseService(
        IGreenhouseRepository greenhouseRepository,
        IPlantRepository plantRepository,
        ISensorRepository sensorRepository,
        IActuatorRepository actuatorRepository)
        : BaseService<Greenhouse>(greenhouseRepository), IGreenhouseService
    {
        public async Task<IEnumerable<Greenhouse>> GetGreenhousesByUserIdAsync(int userId)
        {
            return await greenhouseRepository.GetByUserIdAsync(userId);
        }

        public async Task<IEnumerable<Sensor>> GetSensorsByGreenhouseIdAsync(int greenhouseId)
        {
            return await sensorRepository.GetByGreenhouseIdAsync(greenhouseId);
        }

        public async Task<IEnumerable<Actuator>> GetActuatorsByGreenhouseIdAsync(int greenhouseId)
        {
            return await actuatorRepository.GetByGreenhouseIdAsync(greenhouseId);
        }

        public async Task<IEnumerable<Plant>> GetPlantsByGreenhouseIdAsync(int greenhouseId)
        {
            return await plantRepository.GetByGreenhouseIdAsync(greenhouseId);
        }

        public async Task<Plant> AddPlantToGreenhouseAsync(int greenhouseId, Plant plant)
        {
            var greenhouse = await GetByIdAsync(greenhouseId)
                             ?? throw new KeyNotFoundException($"Greenhouse with ID {greenhouseId} not found");

            greenhouse.AddPlant(plant);
            return await plantRepository.AddAsync(plant);
        }

        public async Task DeletePlantFromGreenhouseAsync(int greenhouseId, int plantId)
        {
            _ = await GetByIdAsync(greenhouseId)
                ?? throw new KeyNotFoundException($"Greenhouse with ID {greenhouseId} not found");

            var plant = await plantRepository.GetByIdAsync(plantId);
            if (plant == null || plant.GreenhouseId != greenhouseId)
                throw new KeyNotFoundException($"Plant with ID {plantId} not found in greenhouse {greenhouseId}");

            await plantRepository.DeleteAsync(plantId);
        }

        public async Task AddSensorToGreenhouseAsync(int greenhouseId, Sensor sensor)
        {
            var greenhouse = await GetByIdAsync(greenhouseId)
                             ?? throw new KeyNotFoundException($"Greenhouse with ID {greenhouseId} not found");

            greenhouse.AddSensor(sensor);
            await sensorRepository.AddAsync(sensor);
        }

        public async Task DeleteSensorFromGreenhouseAsync(int greenhouseId, int sensorId)
        {
            _ = await GetByIdAsync(greenhouseId)
                ?? throw new KeyNotFoundException($"Greenhouse with ID {greenhouseId} not found");

            var sensor = await sensorRepository.GetByIdAsync(sensorId);
            if (sensor == null || sensor.GreenhouseId != greenhouseId)
                throw new KeyNotFoundException($"Sensor with ID {sensorId} not found in greenhouse {greenhouseId}");

            await sensorRepository.DeleteAsync(sensorId);
        }

        public async Task AddActuatorToGreenhouseAsync(int greenhouseId, Actuator actuator)
        {
            var greenhouse = await GetByIdAsync(greenhouseId)
                             ?? throw new KeyNotFoundException($"Greenhouse with ID {greenhouseId} not found");

            greenhouse.AddActuator(actuator);
            //await UpdateAsync(greenhouse);
            await actuatorRepository.AddAsync(actuator);
        }

        public async Task DeleteActuatorFromGreenhouseAsync(int greenhouseId, int actuatorId)
        {
            _ = await GetByIdAsync(greenhouseId)
                ?? throw new KeyNotFoundException($"Greenhouse with ID {greenhouseId} not found");

            var actuator = await actuatorRepository.GetByIdAsync(actuatorId);
            if (actuator == null || actuator.GreenhouseId != greenhouseId)
                throw new KeyNotFoundException($"Actuator with ID {actuatorId} not found in greenhouse {greenhouseId}");

            await actuatorRepository.DeleteAsync(actuatorId);
        }

        public async Task UpdateSensorInGreenhouseAsync(int greenhouseId, int sensorId, Sensor updatedSensor)
        {
            _ = await GetByIdAsync(greenhouseId)
                ?? throw new KeyNotFoundException($"Greenhouse with ID {greenhouseId} not found");

            var existingSensor = await sensorRepository.GetByIdAsync(sensorId);
            if (existingSensor == null || existingSensor.GreenhouseId != greenhouseId)
                throw new KeyNotFoundException($"Sensor with ID {sensorId} not found in greenhouse {greenhouseId}");

            existingSensor.UpdateStatus(updatedSensor.Status);
            await sensorRepository.UpdateAsync(existingSensor);
        }

        public async Task UpdateActuatorInGreenhouseAsync(int greenhouseId, int actuatorId, Actuator updatedActuator)
        {
            _ = await GetByIdAsync(greenhouseId)
                ?? throw new KeyNotFoundException($"Greenhouse with ID {greenhouseId} not found");

            var existingActuator = await actuatorRepository.GetByIdAsync(actuatorId);
            if (existingActuator == null || existingActuator.GreenhouseId != greenhouseId)
                throw new KeyNotFoundException($"Actuator with ID {actuatorId} not found in greenhouse {greenhouseId}");

            existingActuator.UpdateStatus(updatedActuator.Status);
            await actuatorRepository.UpdateAsync(existingActuator);
        }

        public async Task UpdatePlantInGreenhouseAsync(int greenhouseId, int plantId, Plant updatedPlant)
        {
            _ = await GetByIdAsync(greenhouseId)
                ?? throw new KeyNotFoundException($"Greenhouse with ID {greenhouseId} not found");

            var existingPlant = await plantRepository.GetByIdAsync(plantId);
            if (existingPlant == null || existingPlant.GreenhouseId != greenhouseId)
                throw new KeyNotFoundException($"Plant with ID {plantId} not found in greenhouse {greenhouseId}");
            
            existingPlant.UpdateSpecies(updatedPlant.Species);
            existingPlant.UpdateGrowthStage(updatedPlant.GrowthStage);
            
            await plantRepository.UpdateAsync(existingPlant);
        }
    }
}