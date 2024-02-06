using AdvSol.Services.Dtos.CommonCode;
using AdvSol.Utils;
using System.Text.RegularExpressions;

namespace AdvSol.Services
{
    public interface IFieldValidatorService
    {
        Dictionary<string, List<string>> Validate<T>(string entityName, string fieldName, T value, Dictionary<string, List<string>> errors, int rowNum = 0);
        Dictionary<string, List<string>> Validate<T>(string entityName, T entity, Dictionary<string, List<string>> errors, int rowNum = 0, params string[] fieldsToSkip);
        List<CommonCodeDto> CommonCodes { get; set; }
    }
    public class FieldValidatorService : IFieldValidatorService
    {
        List<FieldValidationRule> _rules;
        RegexDefs _regex;
        public List<CommonCodeDto> CommonCodes { get; set; }
        public FieldValidatorService(RegexDefs regex)
        {
            _rules = new List<FieldValidationRule>();
            _regex = regex;

            LoadSystemUserEntityRules();
            LoadStrApplicationEntityRules();
        }

        public IEnumerable<FieldValidationRule> GetFieldValidationRules(string entityName)
        {
            return _rules.Where(x => x.EntityName.ToLowerInvariant() == entityName.ToLowerInvariant());
        }

        private void LoadSystemUserEntityRules()
        {
            _rules.Add(new FieldValidationRule(Entities.SystemUser, Fields.Username, FieldTypes.String, true, 1, 10, null, null, null, null, null, null));
            _rules.Add(new FieldValidationRule(Entities.SystemUser, Fields.Passwrod, FieldTypes.String, true, 8, 255, null, null, null, null, _regex.GetRegexInfo(RegexDefs.Password), null));
            _rules.Add(new FieldValidationRule(Entities.SystemUser, Fields.LastName, FieldTypes.String, true, 1, 30, null, null, null, null, null, null));
            _rules.Add(new FieldValidationRule(Entities.SystemUser, Fields.StreetAddress, FieldTypes.String, true, 1, 255, null, null, null, null, null, null));
            _rules.Add(new FieldValidationRule(Entities.SystemUser, Fields.City, FieldTypes.String, true, 1, 255, null, null, null, null, null, null));
            _rules.Add(new FieldValidationRule(Entities.SystemUser, Fields.Province, FieldTypes.String, true, 1, 255, null, null, null, null, null, null));
            _rules.Add(new FieldValidationRule(Entities.SystemUser, Fields.PostalCode, FieldTypes.String, true, 6, 6, null, null, null, null, null, null));
            _rules.Add(new FieldValidationRule(Entities.SystemUser, Fields.PhoneNumber, FieldTypes.String, true, 14, 14, null, null, null, null, _regex.GetRegexInfo(RegexDefs.PhoneNumber), null));
        }

        private void LoadStrApplicationEntityRules()
        {
            _rules.Add(new FieldValidationRule(Entities.StrApplication, Fields.StreetAddress, FieldTypes.String, true, 1, 255, null, null, null, null, null, null));
            _rules.Add(new FieldValidationRule(Entities.StrApplication, Fields.City, FieldTypes.String, true, 1, 255, null, null, null, null, null, null));
            _rules.Add(new FieldValidationRule(Entities.StrApplication, Fields.Province, FieldTypes.String, true, 1, 255, null, null, null, null, null, null));
            _rules.Add(new FieldValidationRule(Entities.StrApplication, Fields.PostalCode, FieldTypes.String, true, 6, 6, null, null, null, null, null, null));
            _rules.Add(new FieldValidationRule(Entities.StrApplication, Fields.SquareFootage, FieldTypes.String, true, null, null, null, null, null, null, null, null));

            _rules.Add(new FieldValidationRule(Entities.StrApplication, Fields.ZoningTypeId, FieldTypes.String, true, null, null, null, null, null, null, null, CodeSet.ZoneType));
            _rules.Add(new FieldValidationRule(Entities.StrApplication, Fields.StrAffiliateId, FieldTypes.String, true, null, null, null, null, null, null, null, CodeSet.StrAffiliate));
            _rules.Add(new FieldValidationRule(Entities.StrApplication, Fields.ComplianceStatusId, FieldTypes.String, true, null, null, null, null, null, null, null, CodeSet.ComplianceStatus));
        }


        public Dictionary<string, List<string>> Validate<T>(string entityName, T entity, Dictionary<string, List<string>> errors, int rowNum = 0, params string[] fieldsToSkip)
        {
            var fields = typeof(T).GetProperties();

            foreach (var field in fields)
            {
                if (fieldsToSkip.Any(x => x == field.Name))
                    continue;

                Validate(entityName, field.Name, field.GetValue(entity), errors, rowNum);
            }

            return errors;
        }

        public Dictionary<string, List<string>> Validate<T>(string entityName, string fieldName, T val, Dictionary<string, List<string>> errors, int rowNum = 0)
        {
            var rule = _rules.FirstOrDefault(r => r.EntityName == entityName && r.FieldName == fieldName);

            if (rule == null)
                return errors;

            var messages = new List<string>();

            switch (rule.FieldType)
            {
                case FieldTypes.String:
                    messages.AddRange(ValidateStringField(rule, val, rowNum));
                    break;
                case FieldTypes.Date:
                    messages.AddRange(ValidateDateField(rule, val));
                    break;
                default:
                    throw new NotImplementedException($"Validation for {rule.FieldType} is not implemented.");
            }

            if (messages.Count > 0)
            {
                foreach (var message in messages)
                {
                    errors.AddItem(rule.FieldName, message);
                }
            }

            return errors;
        }

        private List<string> ValidateStringField<T>(FieldValidationRule rule, T val, int rowNum = 0)
        {
            var messages = new List<string>();

            var rowNumPrefix = rowNum == 0 ? "" : $"Row # {rowNum}: ";

            var field = rule.FieldName.WordToWords();

            if (rule.Required && val is null)
            {
                messages.Add($"{rowNumPrefix}The {field} field is required.");
                return messages;
            }

            if (!rule.Required && (val is null || val.ToString().IsEmpty()))
                return messages;

            string value = Convert.ToString(val);

            if (rule.Required && value.IsEmpty())
            {
                messages.Add($"{rowNumPrefix}The {field} field is required.");
                return messages;
            }

            if (rule.MinLength != null && rule.MaxLength != null)
            {               
                if (value.Length < rule.MinLength || value.Length > rule.MaxLength)
                {
                    if (rule.MinLength == rule.MaxLength)
                    {
                        messages.Add($"{rowNumPrefix}The length of {field} field must be {rule.MinLength}.");
                    }
                    else
                    {
                        messages.Add($"{rowNumPrefix}The length of {field} field must be between {rule.MinLength} and {rule.MaxLength}.");
                    }
                }
            }

            if (rule.Regex != null)
            {
                if (!Regex.IsMatch(value, rule.Regex.Regex))
                {
                    var message = string.Format(rule.Regex.ErrorMessage, val.ToString());
                    messages.Add($"{rowNumPrefix}{message}.");
                }
            }

            if (rule.CodeSet != null)
            {
                if (decimal.TryParse(value, out decimal numValue))
                {
                    var exists = CommonCodes.Any(x => x.CodeSet == rule.CodeSet && x.Id == numValue);

                    if (!exists)
                    {
                        messages.Add($"{rowNumPrefix}Invalid value. [{value}] doesn't exist in the code set {rule.CodeSet}.");
                    }
                }
                else
                {
                    messages.Add($"{rowNumPrefix}Invalid value. [{value}] doesn't exist in the code set {rule.CodeSet}.");
                }
            }

            return messages;
        }

        private List<string> ValidateDateField<T>(FieldValidationRule rule, T val, int rowNum = 0)
        {
            var messages = new List<string>();

            var rowNumPrefix = rowNum == 0 ? "" : $"Row # {rowNum}: ";

            var field = rule.FieldName.WordToWords();

            if (rule.Required && val is null)
            {
                messages.Add($"{rowNumPrefix}{field} field is required.");
                return messages;
            }

            if (!rule.Required && (val is null || val.ToString().IsEmpty()))
                return messages;

            var (parsed, parsedDate) = DateUtils.ParseDate(val);

            if (!parsed)
            {
                messages.Add($"{rowNumPrefix}Invalid value. [{val.ToString()}] cannot be converted to a date");
                return messages;
            }

            var value = parsedDate;

            if (rule.MinDate != null && rule.MaxDate != null)
            {
                if (value < rule.MinDate || value > rule.MaxDate)
                {
                    messages.Add($"{rowNumPrefix}The length of {field} must be between {rule.MinDate} and {rule.MaxDate}.");
                }
            }

            return messages;
        }
    }
}