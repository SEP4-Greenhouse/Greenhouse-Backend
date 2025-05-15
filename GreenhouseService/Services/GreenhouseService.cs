using Domain.Entities;
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
            var greenhouse = await GetByIdAsync(greenhouseId);
            if (greenhouse == null)
                throw new KeyNotFoundException($"Greenhouse with ID {greenhouseId} not found");

            greenhouse.AddPlant(plant);
            await UpdateAsync(greenhouse);
            return await plantRepository.AddAsync(plant);
        }

        public async Task RemovePlantFromGreenhouseAsync(int greenhouseId, int plantId)
        {
            var greenhouse = await GetByIdAsync(greenhouseId);
            if (greenhouse == null)
                throw new KeyNotFoundException($"Greenhouse with ID {greenhouseId} not found");

            var plant = await plantRepository.GetByIdAsync(plantId);
            if (plant == null || plant.GreenhouseId != greenhouseId)
                throw new KeyNotFoundException($"Plant with ID {plantId} not found in greenhouse {greenhouseId}");

            await plantRepository.DeleteAsync(plantId);
        }

        public async Task AddSensorToGreenhouseAsync(int greenhouseId, Sensor sensor)
        {
            var greenhouse = await GetByIdAsync(greenhouseId);
            if (greenhouse == null)
                throw new KeyNotFoundException($"Greenhouse with ID {greenhouseId} not found");

            greenhouse.AddSensor(sensor);
            await UpdateAsync(greenhouse);
            await sensorRepository.AddAsync(sensor);
        }

        public async Task RemoveSensorFromGreenhouseAsync(int greenhouseId, int sensorId)
        {
            var greenhouse = await GetByIdAsync(greenhouseId);
            if (greenhouse == null)
                throw new KeyNotFoundException($"Greenhouse with ID {greenhouseId} not found");

            var sensor = await sensorRepository.GetByIdAsync(sensorId);
            if (sensor == null || sensor.GreenhouseId != greenhouseId)
                throw new KeyNotFoundException($"Sensor with ID {sensorId} not found in greenhouse {greenhouseId}");

            await sensorRepository.DeleteAsync(sensorId);
        }

        public async Task AddActuatorToGreenhouseAsync(int greenhouseId, Actuator actuator)
        {
            var greenhouse = await GetByIdAsync(greenhouseId);
            if (greenhouse == null)
                throw new KeyNotFoundException($"Greenhouse with ID {greenhouseId} not found");

            greenhouse.AddController(actuator);
            await UpdateAsync(greenhouse);
            await actuatorRepository.AddAsync(actuator);
        }

        public async Task RemoveActuatorFromGreenhouseAsync(int greenhouseId, int actuatorId)
        {
            var greenhouse = await GetByIdAsync(greenhouseId);
            if (greenhouse == null)
                throw new KeyNotFoundException($"Greenhouse with ID {greenhouseId} not found");

            var actuator = await actuatorRepository.GetByIdAsync(actuatorId);
            if (actuator == null || actuator.GreenhouseId != greenhouseId)
                throw new KeyNotFoundException($"Actuator with ID {actuatorId} not found in greenhouse {greenhouseId}");

            await actuatorRepository.DeleteAsync(actuatorId);
        }

        public async Task UpdatePlantTypeAsync(int greenhouseId, string newPlantType)
        {
            var greenhouse = await GetByIdAsync(greenhouseId);
            if (greenhouse == null)
                throw new KeyNotFoundException($"Greenhouse with ID {greenhouseId} not found");

            greenhouse.UpdatePlantType(newPlantType);
            await UpdateAsync(greenhouse);
        }
    }
}