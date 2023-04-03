// Avatar SDK runtime version file
// @generated on 08/04/2022, at 01:12:50 UTC from AndroidManifest.xml
//
// DO NOT MODIFY THIS FILE BY HAND, IT IS AUTOGENERATED

using System;
using System.Runtime.InteropServices;

namespace Oculus.Avatar2
{

  [StructLayout(LayoutKind.Sequential)]
  public struct FBVersionNumber
  {
    public UInt32 releaseVersion;
    public UInt32 hotfixVersion;
    public UInt32 experimentationVersion;
    public UInt32 betaVersion;
    public UInt32 alphaVersion;

    public override string ToString() {
      return $"{releaseVersion}.{hotfixVersion}.{experimentationVersion}.{betaVersion}.{alphaVersion}";
    }
  };

  public static class SDKVersionInfo
  {
    public const UInt32 AVATAR2_RELEASE_VERSION = 16;
    public const UInt32 AVATAR2_HOTFIX_VERSION = 0;
    public const UInt32 AVATAR2_EXPERIMENTATION_VERSION = 0;
    public const UInt32 AVATAR2_BETA_VERSION = 43;
    public const UInt32 AVATAR2_ALPHA_VERSION = 63;

    static public FBVersionNumber CurrentVersion()
    {
        FBVersionNumber versionNumber;
        versionNumber.releaseVersion = AVATAR2_RELEASE_VERSION;
        versionNumber.hotfixVersion = AVATAR2_HOTFIX_VERSION;
        versionNumber.experimentationVersion = AVATAR2_EXPERIMENTATION_VERSION;
        versionNumber.betaVersion = AVATAR2_BETA_VERSION;
        versionNumber.alphaVersion = AVATAR2_ALPHA_VERSION;

        return versionNumber;
    }
  }
}
