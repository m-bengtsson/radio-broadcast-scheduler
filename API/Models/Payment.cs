
public class Payment
{
   public Guid Id { get; private set; }
   public DateTime Date { get; set; }
   public decimal Amount { get; set; }
   public Guid ContributorId { get; private set; }
   public Contributor Contributor { get; private set; }

   private Payment() { } // For EF Core
   public Payment(DateTime date, decimal amount, Contributor contributor)
   {
      Id = Guid.NewGuid();
      Date = date;
      Amount = amount;
      // Contributor = contributor ?? throw new ArgumentNullException(nameof(contributor));
      ContributorId = contributor.Id;

   }
}