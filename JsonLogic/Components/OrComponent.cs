﻿using System.Text.Json;
using Json.More;

namespace Json.Logic.Components
{
	internal class OrComponent : ILogicComponent
	{
		private readonly ILogicComponent _a;
		private readonly ILogicComponent _b;

		public OrComponent(ILogicComponent a, ILogicComponent b)
		{
			_a = a;
			_b = b;
		}

		public JsonElement Apply(JsonElement data)
		{
			return (_a.Apply(data).IsTruthy() ||
			        _b.Apply(data).IsTruthy()).AsJsonElement();
		}
	}
}