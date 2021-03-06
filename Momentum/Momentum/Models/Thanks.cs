﻿using Amazon.DynamoDBv2.DataModel;
using Momentum.Converters;
using Momentum.Extensions;
using Momentum.Primitives;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Momentum.Models
{
    public interface IThanks
    {
        string UserId { get; set; }
        string ThanksId { get; set; }
        string MomentId { get; set; }
        Position Position { get; set; }
        DateTime Timestamp { get; set; }
        Address Address { get; set; }
    }

    [DynamoDBTable("momentum-mobilehub-268650841-thanks")]
    public class Thanks : IThanks
    {
        [DynamoDBHashKey("userId")]
        public string UserId { get; set; }

        [DynamoDBRangeKey("thanksId")]
        public string ThanksId { get; set; }

        [DynamoDBProperty("momentId")]
        public string MomentId { get; set; }

        [DynamoDBProperty("date")]
        public string Date { get; set; }

        [DynamoDBProperty("time")]
        public string Time { get; set; }

        [DynamoDBProperty("position", typeof(PositionPropertyConverter))]
        public Position Position { get; set; }

        [DynamoDBProperty("city")]
        public string City { get; set; }

        [DynamoDBProperty("postalCode")]
        public string PostalCode { get; set; }

        [DynamoDBProperty("countryCode")]
        public string CountryCode { get; set; }

        [DynamoDBProperty("countryName")]
        public string CountryName { get; set; }

        [DynamoDBIgnore]
        public DateTime Timestamp
        {
            get
            {
                return DateTime.ParseExact(string.Format("{0}T{1}Z", Date, Time), "yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
            }
            set
            {
                Date = value.ToDateString();
                Time = value.ToTimeString();
            }
        }

        [DynamoDBIgnore]
        public Address Address
        {
            get
            {
                return new Address { City = City, PostalCode = PostalCode, CountryName = CountryName, CountryCode = CountryCode };
            }
            set
            {
                City = value?.City;
                PostalCode = value?.PostalCode;
                CountryName = value?.CountryName;
                CountryCode = value?.CountryCode;
            }
        }

        public static Thanks Create(Moment moment)
        {
            return new Thanks()
            {
                ThanksId = Guid.NewGuid().ToString(),
                MomentId = moment.MomentId,
                UserId = moment.UserId,
                Timestamp = DateTime.UtcNow
            };
        }
    }
}
