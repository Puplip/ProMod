using ProMod.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ProMod.HUD.Elements;

[ProHUDElement("NewLine", ProHUD.ElementType.NewLine)]
public class ProNewLine : ProHUDElementBase
{
}
[ProHUDElement("VerticalSpacer4", 0, 4, ProHUD.ElementType.VerticalSeparator)]
public class ProVerticalSpacer4 : ProHUDElementBase
{
}

[ProHUDElement("VerticalSpacer8", 0, 8, ProHUD.ElementType.VerticalSeparator)]
public class ProVerticalSpacer8 : ProHUDElementBase
{
}

[ProHUDElement("VerticalSpacer16", 0, 16, ProHUD.ElementType.VerticalSeparator)]
public class ProVerticalSpacer16 : ProHUDElementBase
{
}

[ProHUDElement("VerticalSpacer24", 0, 24, ProHUD.ElementType.VerticalSeparator)]
public class ProVerticalSpacer24 : ProHUDElementBase
{
}

[ProHUDElement("HorizontalSpacer16", 12, 0, ProHUD.ElementType.HorizontalSeparator)]
public class ProHorizontalSpacer16 : ProHUDElementBase
{
}

[ProHUDElement("Empty", 0, 0, ProHUD.ElementType.Normal)]
public class ProEmpty : ProHUDElementBase
{
}