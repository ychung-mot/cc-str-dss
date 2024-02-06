﻿using System.Text.Json.Serialization;

namespace AdvSol.Services.Dtos.StrApplication
{
    public class StrApplicationCreateDto : IAddress
    {
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public int ZoningTypeId { get; set; }
        public int SquareFootage { get; set; }
        public int StrAffiliateId { get; set; }
        public bool IsOwnerPrimaryResidence { get; set; }
        [JsonIgnore]
        public double? Longitude { get; set; }
        [JsonIgnore]
        public double? Latitude { get; set; }
    }
}