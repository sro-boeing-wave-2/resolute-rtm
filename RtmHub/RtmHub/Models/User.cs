using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RtmHub.Models
{

    public class User
    {
        int id;
        string name;
        string email;
        string phone_no;
        string profile_img_url;
        Customer organization;
        DateTime createdOn;
        long createdBy;
        DateTime updatedOn;
        long updatedBy;
        [Key]
        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Email { get => email; set => email = value; }
        public string Phone_no { get => phone_no; set => phone_no = value; }
        public string Profile_img_url { get => profile_img_url; set => profile_img_url = value; }
        public Customer Organization { get => organization; set => organization = value; }
        public DateTime CreatedOn { get => createdOn; set => createdOn = value; }
        public long CreatedBy { get => createdBy; set => createdBy = value; }
        public DateTime UpdatedOn { get => updatedOn; set => updatedOn = value; }
        public long UpdatedBy { get => updatedBy; set => updatedBy = value; }
    }

    public class EndUser : User
    {
        List<UserSocialId> socialId;

        public List<UserSocialId> SocialId { get => socialId; set => socialId = value; }
    }

    public class Customer
    {
        int id;
        string customer_name;
        string email;
        string password;
        string logo_url;
        DateTime createdOn;
        long createdBy;
        DateTime updatedOn;
        long updatedBy;
        [Key]
        public int Id { get => id; set => id = value; }
        public string Customer_name { get => customer_name; set => customer_name = value; }
        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }
        public string Logo_url { get => logo_url; set => logo_url = value; }
        public DateTime CreatedOn { get => createdOn; set => createdOn = value; }
        public long CreatedBy { get => createdBy; set => createdBy = value; }
        public DateTime UpdatedOn { get => updatedOn; set => updatedOn = value; }
        public long UpdatedBy { get => updatedBy; set => updatedBy = value; }
    }

    public class Department
    {
        int departmentId;
        string department;
        DateTime createdOn;
        long createdBy;
        DateTime updatedOn;
        long updatedBy;

        [Key]
        public int DepartmentId { get => departmentId; set => departmentId = value; }
        public string DepartmentName { get => department; set => department = value; }
        public DateTime CreatedOn { get => createdOn; set => createdOn = value; }
        public long CreatedBy { get => createdBy; set => createdBy = value; }
        public DateTime UpdatedOn { get => updatedOn; set => updatedOn = value; }
        public long UpdatedBy { get => updatedBy; set => updatedBy = value; }
    }

    public class Agent : User
    {
        Department department;

        public Department Department { get => department; set => department = value; }
    }

    public class UserSocialId
    {
        int socialId;
        string source;
        string identifier;
        DateTime createdOn;
        long createdBy;
        DateTime updatedOn;
        long updatedBy;

        [Key]
        public int SocialId { get => socialId; set => socialId = value; }
        public string Source { get => source; set => source = value; }
        public string Identifier { get => identifier; set => identifier = value; }
        public DateTime CreatedOn { get => createdOn; set => createdOn = value; }
        public long CreatedBy { get => createdBy; set => createdBy = value; }
        public DateTime UpdatedOn { get => updatedOn; set => updatedOn = value; }
        public long UpdatedBy { get => updatedBy; set => updatedBy = value; }
}

}
