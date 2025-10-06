using _1_API.Response;
using AutoMapper;
using Domain;
using NutriControl.Domain.Fields.Models.Entities;


namespace _1_API.Mapper;

public class ModelsToResponse : Profile
{
    public ModelsToResponse()
    {

        CreateMap<User, UserResponse>();
        CreateMap<Subscription, SubscriptionResponse>();
        CreateMap<Field, FieldResponse>();
        CreateMap<Crop, CropResponse>();
        CreateMap<Recommendation, RecommendationResponse>();
        CreateMap<History, HistoryResponse>();
        CreateMap<Device, DeviceResponse>();
        CreateMap<Sensor, SensorResponse>();
        CreateMap<SensorReading, SensorReadingResponse>();
        CreateMap<Alert, AlertResponse>();


    }
}