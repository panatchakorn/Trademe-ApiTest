# Trademe-ApiTest
This api project tests Trademe Sandbox (https://api.tmsandbox.co.nz). It is implemented using VisualStudio 2019 (Community version). 
It is a C# .NET Framework 4.6.1 Test Project using SpecFlow and RestSharp. 

The project is to test that user can retrieve charities list, search and view used car listing via api calls.

<b>---- Pre-requisites ----</b>

- Install Visual Studio 2019 (I use community version) with:

  -- NET desktop development (from installation details section > tick .NET Framework 4.6.1 development tools)
  
  -- Universal Windows Platform development
  
- Open Visual Studio 2019 without launching any project > Go to Extensions > Manage Extensions

- Search, download and install:

  -- GitHub Extension for Visual Studio

  -- SpecFlow for Visual Studio 2019
  
- Chrome (Version 88.0.4324.104) or Edge Browser (88.0.705.56). These are the versions I use at the time of creating test. This is for user authentication.

<b>---- Clone project ----</b>

- From Visual studio clone a new project from https://github.com/panatchakorn/Trademe-ApiTest. It will download and restore required packages:

<b>---- Rebuild Trademe Solution and run the test ----</b>

- Right click on the Trademe Solution > Select Clean Solution

- Right click on the Trademe Solution > Select Build Solution

- From Test Explorer > Select ApiTest and Run the test

  A window pop up asking to enter 7 digit pin. The pop up might goes behind, pleaes check.
  
  A browser will pop up and direct you to trademe website. Please login and allow PJTestApp then it will show 7 digit pin.
  
  Enter 7 digit in the window pop up and let the test run
  
  <b>Note</b> <i>You will have to allow PJTestApp once, but will require to login to trademe and get 7 digit pin for each test run.</i>
  
 - View the result in Test Explorer. There will be some failed tests. See Known issue below.

<b>---- Manual Reference Download from Nuget-----</b>

In the case where it has failed to automatically restore the packages, you can download it via Nuget

- Expand ApiTest Project > References > Packages to see current references

- If there are any missing references, Right Click on ApiTest project > Select Manage NuGet Packages

- Go to Browse section, search and install missing references. Here are the list.
<table>
<tr>

<td>

  -- "BoDi" version="1.4.1" targetFramework="net461"
  
  -- "Cucumber.Messages" version="6.0.1" targetFramework="net461
  
  -- "Gherkin" version="6.0.0" targetFramework="net461"
  
  -- "Google.Protobuf" version="3.7.0" targetFramework="net461" 
  
  -- "MSTest.TestAdapter" version="2.1.1" targetFramework="net461"
  
  -- "MSTest.TestFramework" version="2.1.1" targetFramework="net461" 
  
  -- "Newtonsoft.Json" version="12.0.3" targetFramework="net461"
  
  -- "NUnit" version="3.13.0" targetFramework="net461"
  
  -- "NUnit3TestAdapter" version="3.17.0" targetFramework="net461" 
  
  -- "RestSharp" version="106.11.7" targetFramework="net461"
  
  -- "SpecFlow" version="3.5.5" targetFramework="net461"
  
  -- "SpecFlow.NUnit" version="3.5.5" targetFramework="net461" 
  
  -- "SpecFlow.Tools.MsBuild.Generation" version="3.5.5" targetFramework="net461" 
  
  -- "System.Reflection.Emit" version="4.3.0" targetFramework="net461" 
  
  -- "System.Reflection.Emit.Lightweight" version="4.3.0" targetFramework="net461" 
  
  -- "System.Threading.Tasks.Extensions" version="4.4.0" targetFramework="net461" 
  
  -- "System.ValueTuple" version="4.4.0" targetFramework="net461"
  
  -- "Utf8Json" version="1.3.7" targetFramework="net461"

</td>
</tr>
</table>

- Rebuild then run the test again

<b>---- Known Issue ----</b>

I have added comment about not getting OK response in some api and what I have tried to resolve it in the SearchUsedCar.feature file.

Briefly I am not able to get OK response for used car search api. I have the same problem when doing general search api with a query search parameter. 

However general search api is working fine without query parameter. Retriving charities list and view specific used car listing are fine too.

Pleae let me know if you have the solution. Thanks in advance.





