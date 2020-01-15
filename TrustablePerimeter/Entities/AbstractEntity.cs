using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace TrustablePerimeter.Entities
{
    public abstract class AbstractEntity
    {
        public void Update(object newEntity)
        {
            Type entityType = newEntity.GetType();
            PropertyInfo[] entityProperties = entityType.GetProperties();
            foreach (PropertyInfo property in entityProperties)
            {
                object newValue = property.GetValue(newEntity);
                property.SetValue(this, newValue);
            }
        }

        public void Update(IDictionary<string, object> newProperties)
        {
            Type entityType = this.GetType();
            PropertyInfo[] givenProperties = entityType.GetProperties();
            foreach (string key in newProperties.Keys)
            {
                if (givenProperties.Any((PropertyInfo property) => property.Name.ToLower() == key.ToLower()))
                {
                    PropertyInfo updatedProperty = givenProperties.Single((PropertyInfo property) => property.Name.ToLower() == key.ToLower());
                    updatedProperty.SetValue(this, Convert.ChangeType(newProperties[key], updatedProperty.PropertyType));
                }
            }
        }
    }
}