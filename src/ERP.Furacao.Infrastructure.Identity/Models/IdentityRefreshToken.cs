using System;

namespace ERP.Furacao.Infrastructure.Identity.Models
{
    public class IdentityRefreshToken<TKey>
    {
        public virtual int Id { get; set; }
        public virtual TKey UserId { get; set; }
        public virtual string Token { get; set; }
        public virtual DateTime Expires { get; set; }
        public virtual bool IsExpired => DateTime.UtcNow >= Expires;
        public virtual DateTime Created { get; set; }
        public virtual string CreatedByIp { get; set; }
        public virtual DateTime? Revoked { get; set; }
        public virtual string RevokedByIp { get; set; }
        public virtual string ReplacedByToken { get; set; }
        public virtual bool IsActive => Revoked == null && !IsExpired;
    }
}
