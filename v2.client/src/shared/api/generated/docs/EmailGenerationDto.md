# EmailGenerationDto


## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**template** | **string** |  | [optional] [default to undefined]
**sender** | [**UserData**](UserData.md) |  | [optional] [default to undefined]
**recipients** | [**Array&lt;UserData&gt;**](UserData.md) |  | [optional] [default to undefined]

## Example

```typescript
import { EmailGenerationDto } from './api';

const instance: EmailGenerationDto = {
    template,
    sender,
    recipients,
};
```

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)
