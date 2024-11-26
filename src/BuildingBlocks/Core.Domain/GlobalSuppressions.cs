// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Major Code Smell", "S3993:Custom attributes should be marked with \"System.AttributeUsageAttribute\"", Justification = "<Pendente>", Scope = "type", Target = "~T:Core.Domain.DataAnnotations.RequiredGuidAttribute")]
[assembly: SuppressMessage("Major Bug", "S1751:Loops with at most one iteration should be refactored", Justification = "<Pendente>", Scope = "member", Target = "~M:Core.Domain.Entities.Entity.Validar(FluentValidation.Results.ValidationResult)")]
[assembly: SuppressMessage("Major Code Smell", "S127:\"for\" loop stop conditions should be invariant", Justification = "<Pendente>", Scope = "member", Target = "~M:Core.Domain.Validacoes.DigitoVerificador.GetDigitSum~System.String")]
