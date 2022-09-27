using Application.Payment.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Payment
{
    public interface IPaymentService
    {
        Task<PaymentStateDto> PayWithCreditCard(string paymentIntention);
        Task<PaymentStateDto> PayWithDebitCard(string paymentIntention);
        Task<PaymentStateDto> PayBankTransfer(string paymentIntention);
    }
}
