﻿using System.Text.Json.Serialization;
using Domain;

namespace _1_API.Response;

public class SensorResponse
{
    public int Id { get; set; } 
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public SensorType Type { get; set; }
    
    public string UnitOfMeasurement { get; set; }
    
    public string Status { get; set; }
}