# EventDto


## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**id** | **string** |  | [optional] [default to undefined]
**title** | **string** |  | [optional] [default to undefined]
**description** | **string** |  | [optional] [default to undefined]
**categoryId** | **number** |  | [optional] [default to undefined]
**eventDate** | **string** |  | [optional] [default to undefined]
**participates** | [**Array&lt;UserDto&gt;**](UserDto.md) |  | [optional] [default to undefined]

## Example

```typescript
import { EventDto } from './api';

const instance: EventDto = {
    id,
    title,
    description,
    categoryId,
    eventDate,
    participates,
};
```

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)
