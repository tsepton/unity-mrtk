
using System;
using UnityEngine.Events;

public interface CustomGesture {
  float ThumbCurl { get; } 
  float IndexCurl { get; } 
  float MiddleCurl { get; } 
  float RingCurl { get; } 
  float PinkyCurl { get; }

  public bool IsOccuring();
}

[Serializable] 
public class HandWideOpen: UnityEvent, CustomGesture {
  public float ThumbCurl { get; } = 0;

  public float IndexCurl { get; } = 0;

  public float MiddleCurl { get; } = 0;

  public float RingCurl { get; } = 0;

  public float PinkyCurl { get; } = 0;
  
  public bool IsOccuring() {
    throw new NotImplementedException();
  }
  
  private HandWideOpen() {}

  public static HandWideOpen Instance => Singleton.instance;

  private class Singleton
  {
    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit
    static Singleton() {}

    internal static readonly HandWideOpen instance = new HandWideOpen();
  }
}

[Serializable] 
public class HandItalian : UnityEvent, CustomGesture {
  public float ThumbCurl { get; } = 0.7f;

  public float IndexCurl { get; } = 0.35f;

  public float MiddleCurl { get; } = 0.35f;

  public float RingCurl { get; } = 0.35f;

  public float PinkyCurl { get; } = 0.3f;
  
  public bool IsOccuring() {
    throw new NotImplementedException();
  }
  
  private HandItalian() {}

  public static HandItalian Instance => Singleton.instance;

  private class Singleton
  {
    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit
    static Singleton() {}

    internal static readonly HandItalian instance = new HandItalian();
  }
}

