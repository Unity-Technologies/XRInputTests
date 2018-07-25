using System.Text;
using UnityEngine;

public class TestInstructions : MonoBehaviour  //, ITestTextProvider
{
    [SerializeField, TextArea] string m_Instructions;
    [SerializeField, TextArea] string m_StandaloneInstructions;
    [SerializeField, TextArea] string m_MobileInstructions;

    public int testTextOrder { get { return 0; } }

    public string GetFullString()
    {
        return m_Instructions + "  " + (Application.isMobilePlatform ? m_MobileInstructions : m_StandaloneInstructions);
    }
}
