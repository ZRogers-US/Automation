#include "Tests/AutomationEditorCommon.h" // requiredforFAutomationEditorCommonUtils along with the "UnrealEd" module
#include "Developer/AutomationDriver/Public/AutomationDriverCommon.h" // required for IAutomationDriverModule along with the, "AutomationDriver" module



IMPLEMENT_SIMPLE_AUTOMATION_TEST(InteractableResourcesSpawnedTest, "AutomationDriver.MainMenu.NewGame",EAutomationTestFlags::EditorContext | EAutomationTestFlags::ProductFilter)



bool InteractableResourcesSpawnedTest::RunTest(const FString& Parameters)
{
	bool result = false;

	FString MapPath = FPaths::ProjectContentDir() / TEXT("MainMenu.umap");
	FAutomationEditorCommonUtils::LoadMap(MapPath);

	IAutomationDriverModule::Get().Enable();

	FAutomationDriverPtr Driver = IAutomationDriverModule::Get().CreateDriver();

// was unable to find UI element, unsure but might need the UI created in C++ first in order to add metadata to the element 
/*
	FDriverElementRef NewGameBtn = Driver->FindElement(By::Id("BTN_NewGame"));
	if (NewGameBtn->Exists())
	{
		UE_LOG(LogTemp, Warning, TEXT("new game button exists"));
		NewGameBtn->Click();
	}
/*

	IAutomationDriverModule::Get().Disable();

	return result;
}