﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sino.Extensions.EventBus.Configuration
{
    public class PublishConfigurationBuilder : IPublishConfigurationBuilder
    {
        private readonly ExchangeConfigurationBuilder _exchange;
        private string _routingKey;
        private Action<IBasicProperties> _properties;
        private const string _oneOrMoreWords = "#";
        private EventHandler<BasicReturnEventArgs> _basicReturn;

        public PublishConfiguration Configuration => new PublishConfiguration
        {
            Exchange = _exchange.Configuration,
            RoutingKey = _routingKey,
            PropertyModifier = _properties ?? (b => { }),
            BasicReturn = _basicReturn
        };

        public PublishConfigurationBuilder(ExchangeConfiguration defaultExchange = null, string routingKey = null)
        {
            _exchange = new ExchangeConfigurationBuilder(defaultExchange);
            _routingKey = routingKey ?? _oneOrMoreWords;
        }

        public IPublishConfigurationBuilder WithExchange(Action<IExchangeConfigurationBuilder> exchange)
        {
            exchange(_exchange);
            Configuration.Exchange = _exchange.Configuration;
            return this;
        }

        public IPublishConfigurationBuilder WithRoutingKey(string routingKey)
        {
            _routingKey = routingKey;
            return this;
        }

        public IPublishConfigurationBuilder WithProperties(Action<IBasicProperties> properties)
        {
            _properties = properties;
            return this;
        }

        public IPublishConfigurationBuilder WithMandatoryDelivery(EventHandler<BasicReturnEventArgs> basicReturn)
        {
            _basicReturn = basicReturn;
            return this;
        }
    }
}
