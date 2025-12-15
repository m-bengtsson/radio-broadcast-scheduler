public class PaymentDto
{
   public Guid Id { get; set; }
   public decimal NetAmount { get; set; }
   public DateOnly BillingPeriod { get; set; }

}