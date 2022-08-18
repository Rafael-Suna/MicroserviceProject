using FreeCourse.Web.Models.FakePayments;
using System.Threading.Tasks;

namespace FreeCourse.Web.Services.Interfaces
{
    public interface IPaymentService
    {
       public Task<bool> ReceivePayment(PaymentInfoInput paymentInfoInput);
    }
}
