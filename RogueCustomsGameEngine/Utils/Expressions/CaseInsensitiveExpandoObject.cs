using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#pragma warning disable CS8601 // Posible asignación de referencia nula
#pragma warning disable CS8765 // La nulabilidad del tipo de parámetro no coincide con el miembro invalidado (posiblemente debido a los atributos de nulabilidad).

namespace RogueCustomsGameEngine.Utils.Expressions
{
    public class CaseInsensitiveExpandoObject : DynamicObject
    {
        private readonly Dictionary<string, object> _properties = new (StringComparer.OrdinalIgnoreCase);

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return _properties.TryGetValue(binder.Name, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _properties[binder.Name] = value;
            return true;
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return _properties.Keys;
        }

        public IDictionary<string, object> ToDictionary()
        {
            return _properties.ToDictionary(d => d.Key, d => d.Value);
        }

        public void Populate(IDictionary<string, object> dict)
        {
            foreach (var kvp in dict)
            {
                this.TrySetMember(new SetMemberBinderImpl(kvp.Key), kvp.Value);
            }
        }

        // A simple implementation for SetMemberBinder
        private class SetMemberBinderImpl : SetMemberBinder
        {
            public SetMemberBinderImpl(string name) : base(name, true) { }
            public override DynamicMetaObject FallbackSetMember(DynamicMetaObject target, DynamicMetaObject value, DynamicMetaObject errorSuggestion)
            {
                return value; // Just a simple implementation
            }
        }
    }
}
#pragma warning restore CS8765 // La nulabilidad del tipo de parámetro no coincide con el miembro invalidado (posiblemente debido a los atributos de nulabilidad).
#pragma warning restore CS8601 // Posible asignación de referencia nula
