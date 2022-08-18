using FreeCourse.Services.Basket.Services;
using FreeCourse.Shared.Messages;
using FreeCourse.Shared.Services;
using MassTransit;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Services.Basket.Consumers
{
    public class CourseNameChangedEventConsumer : IConsumer<CourseNameChangedEvent>
    {
        private readonly IBasketService _basketService;
   

        public CourseNameChangedEventConsumer(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public async Task Consume(ConsumeContext<CourseNameChangedEvent> context)
        {
            var basketDto = _basketService.GetBasket(context.Message.UserId).Result.Data;
            basketDto.basketItems.Where(x => x.CourseId == context.Message.CourseId).FirstOrDefault().CourseName=context.Message.UpdatedName;
       
            await _basketService.SaveOrUpdate(basketDto);

        }
    }
}