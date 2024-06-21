using Core.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Student:AuditEntity
    {
        public string FullName {  get; set; }
        public string Email { get; set; }
        public DateTime BirthDay { get; set; }
        public int GroupId {  get; set; }
        public Group Group { get; set; }
        public string Image {  get; set; }
        [NotMapped]
        public IFormFile? FormFile { get; set; }
    }
}
