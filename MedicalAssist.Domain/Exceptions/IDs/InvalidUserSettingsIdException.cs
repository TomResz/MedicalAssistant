using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalAssist.Domain.Exceptions.IDs;
public sealed class InvalidUserSettingsIdException : BadRequestException
{
	public InvalidUserSettingsIdException() : base("Invalid user setting Id.")
	{
	}
}
