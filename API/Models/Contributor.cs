using System.Net.Mail;


public class Contributor
{
   public Guid Id { get; private set; }
   public string Name { get; set; }
   public string Address { get; set; }
   public string PhoneNumber { get; set; }
   public string Email { get; set; }

   public List<Payment> Payments { get; set; } = [];
   public List<BroadcastContent> Broadcasts { get; set; } = [];


   private Contributor() { }
   public Contributor(string name, string address, string phoneNumber, string email)
   {
      Id = Guid.NewGuid();
      Name = name;
      Address = address;
      PhoneNumber = phoneNumber;
      Email = email;
   }

   public void AddPayment(Payment payment)
   {
      if (payment == null)
         throw new ArgumentNullException(nameof(payment));
      Payments.Add(payment);
   }
}