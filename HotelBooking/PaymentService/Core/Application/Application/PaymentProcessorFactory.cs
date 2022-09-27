using Application.Booking.DTO;
using Application.Payment.Ports;
using Payments.Application.MercadoPago;

namespace Payments.Application
{
    public class PaymentProcessorFactory : IPaymentProcessorFactory
    {

        public IPaymentProcessor GetPaymentProcessor(SupportedPaymentProviders selectedPaymentProvider)
        {
            switch (selectedPaymentProvider)
            {
                case SupportedPaymentProviders.MercadoPago:
                    return new MercadoPagoAdapter();

                default: return new NotImplementedPaymentProvider();
            }
        }
    }
}
