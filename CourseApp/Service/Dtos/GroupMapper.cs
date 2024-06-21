using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dtos
{
    internal class GroupMapper
    {
        public static Group GroupCreate(GroupCreateDto createDto)
        {
            var group = new Group();

            Type dtoType = createDto.GetType();
            Type entityType = typeof(Group);

            PropertyInfo[] dtoProperties = dtoType.GetProperties();
            foreach (var dtoProperty in dtoProperties)
            {
                PropertyInfo entityProperty = entityType.GetProperty(dtoProperty.Name);
                if (entityProperty != null && entityProperty.CanWrite)
                {
                    var value = dtoProperty.GetValue(createDto);
                    entityProperty.SetValue(group, value);
                }
            }
            return group;
        }

        public static GroupGetDto GroupGetDto(Group group)
        {
            var groupGetDto = new GroupGetDto();

            Type groupType = group.GetType();
            Type dtoType = typeof(GroupGetDto);

            PropertyInfo[] groupProperties = groupType.GetProperties();
            foreach (var groupProperty in groupProperties)
            {
                PropertyInfo dtoProperty = dtoType.GetProperty(groupProperty.Name);
                if (dtoProperty != null && dtoProperty.CanWrite)
                {
                    var value = groupProperty.GetValue(group);
                    dtoProperty.SetValue(groupGetDto, value);
                }
            }
            return groupGetDto;
        }
    }
}
