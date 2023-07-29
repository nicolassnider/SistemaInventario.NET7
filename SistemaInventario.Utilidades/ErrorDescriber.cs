using Microsoft.AspNetCore.Identity;

namespace SistemaInventario.Utilidades
{
    public class ErrorDescriber:IdentityErrorDescriber
    {
        public override IdentityError PasswordRequiresDigit()
        {
            return new IdentityError()
            {
                Code = nameof(PasswordRequiresDigit),
                Description = "Password requiere un dígito"
            };
        }
    }
}
