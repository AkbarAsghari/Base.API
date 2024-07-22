using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace API.Infrastructure.Entities
{
    public class PaymentStatus
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
    }

    public class PaymentStatusEntityTypeConfiguration : IEntityTypeConfiguration<PaymentStatus>
    {
        public void Configure(EntityTypeBuilder<PaymentStatus> builder)
        {
            builder.HasData(new List<PaymentStatus>
            {
                new PaymentStatus { Id = -1 , Description = "منتظر پرداخت" },
                new PaymentStatus { Id = -2 , Description = "خطای داخلی" },
                new PaymentStatus { Id = 1 , Description = "پرداخت شده - تایید" },
                new PaymentStatus { Id = 2 , Description = "پرداخت شده - عدم تایید" },
                new PaymentStatus { Id = 3 , Description = "انصراف از طرف کاربر" },
                new PaymentStatus { Id = 4 , Description = "شماره کارت تا معتبر میباشد" },
                new PaymentStatus { Id = 5 , Description = "موجودی حساب کافی نیست" },
                new PaymentStatus { Id = 6 , Description = "رمز وارد شده اشتباه است" },
                new PaymentStatus { Id = 7 , Description = "تعداد درخواست ها بیش از حد مجاز است" },
                new PaymentStatus { Id = 8 , Description = "تعداد پرداخت های آنلاین در روز بیش از حد مجاز است" },
                new PaymentStatus { Id = 9 , Description = "میزان پرداخت اینترنتی روزانه بیش از حد مجاز می باشد" },
                new PaymentStatus { Id = 10 , Description = "صادر کننده کارت نامعتبر است" },
                new PaymentStatus { Id = 11 , Description = "خطای سوئیچ" },
                new PaymentStatus { Id = 12 , Description = "کارت در دسترس نمیباشد" },
            });
        }
    }
}
