using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;

namespace NSideEffects
{
    public class ProjectParser
    {
        public void Parse(string path)
        {
            ModuleDefinition moduleDefinition = ModuleDefinition.ReadModule(path);
            moduleDefinition.Types.ToList().ForEach(VisitEachType);
        }

        private void VisitEachType(TypeDefinition typeDefinition)
        {
            typeDefinition.Methods.ToList().ForEach(VisitMethod);
        }
        
        private void VisitMethod(MethodDefinition method)
        {
            if (method.Body == null) return;
            Collection<Instruction> instructions = method.Body.Instructions;
            IEnumerable<Instruction> functionCalls = instructions.ToList().FindAll(IsFunctionCall);
            IEnumerable<Instruction> stateChangingFunctionCalls = functionCalls.ToList().FindAll(instruction => IsSetterCallsOnOtherTypes(instruction, method.DeclaringType)) ;
            if(stateChangingFunctionCalls.Count() > 0)
                Console.WriteLine(method.FullName);
        }

        private bool IsFunctionCall(Instruction instruction)
        {
            return instruction.OpCode == OpCodes.Call || instruction.OpCode == OpCodes.Callvirt;
        }

        private bool IsSetterCallsOnOtherTypes(Instruction instruction, TypeDefinition declaringType)
        {
            var methodCalled = (MethodReference)instruction.Operand;
            if (!PropertySetter(methodCalled) || CallingSystemMethod(methodCalled)) return false;
            return methodCalled.DeclaringType != declaringType &&
                   !declaringType.IsSubClassOf(methodCalled.DeclaringType);
        }

        private bool PropertySetter(MethodReference methodCalled)
        {
            return methodCalled.Name.StartsWith("set_");
        }

        private bool CallingSystemMethod(MethodReference methodCalled)
        {
            return methodCalled.DeclaringType.FullName.Contains("System.");
        }
    }

    public static class TypeReferenceX
    {
        public static bool IsSubClassOf(this TypeReference type, TypeReference parentType)
        {
            if (type.IsDefinition && parentType.IsDefinition)
                return ((TypeDefinition) type).BaseType == parentType;
            return false;
        }
    }
}