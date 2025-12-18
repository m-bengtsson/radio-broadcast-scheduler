using Microsoft.AspNetCore.Identity;


public class Contributor
{
   public Guid Id { get; private set; }

   public string UserId { get; private set; }
   public IdentityUser User { get; private set; }
   public string Name { get; set; }
   public string Address { get; set; }
   public string PhoneNumber { get; set; }
   public string Email { get; set; }

   public List<Payment> Payments { get; set; } = [];
   public List<EventContent> Events { get; set; } = [];


   private Contributor() { }
   public Contributor(IdentityUser user, string name, string address, string phoneNumber, string email)
   {
      Id = Guid.NewGuid();
      User = user ?? throw new ArgumentNullException(nameof(user));
      UserId = user.Id;

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