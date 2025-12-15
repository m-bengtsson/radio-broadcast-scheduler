
public class Payment
{
   public Guid Id { get; private set; }
   public DateOnly BillingPeriod { get; private set; }
   public decimal NetAmount { get; private set; }
   public decimal VatAmount { get; private set; }
   public decimal GrossAmount { get; private set; }

   public decimal Hours { get; private set; }
   public int EventCount { get; private set; }


   public Guid ContributorId { get; private set; }
   public Contributor Contributor { get; private set; }


   private Payment() { } // For EF Core
   public Payment(DateOnly billingPeriod, Contributor contributor, decimal hours, int eventCount)
   {
      Id = Guid.NewGuid();
      Contributor = contributor ?? throw new ArgumentNullException(nameof(contributor)); // Ensure valid contributor
      ContributorId = contributor.Id;
      BillingPeriod = billingPeriod;

      Hours = hours;
      NetAmount = (hours * PaymentSettings.HourlyRate) +
                    (eventCount * PaymentSettings.EventFee);
      EventCount = eventCount;
      VatAmount = NetAmount * PaymentSettings.VatRate;
      GrossAmount = NetAmount + VatAmount;
   }
}

public static class PaymentSettings
{
   public const decimal HourlyRate = 750m;
   public const decimal EventFee = 300m;
   public const decimal VatRate = 0.25m;

}