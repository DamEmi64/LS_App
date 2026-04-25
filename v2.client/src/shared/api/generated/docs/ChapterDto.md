# ChapterDto


## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**heroes** | [**Array&lt;HeroDto&gt;**](HeroDto.md) |  | [optional] [default to undefined]
**places** | [**Array&lt;PlaceDto&gt;**](PlaceDto.md) |  | [optional] [default to undefined]
**id** | **string** |  | [optional] [default to undefined]
**title** | **string** |  | [default to undefined]
**description** | **string** |  | [default to undefined]
**story** | **string** |  | [optional] [default to undefined]
**startDate** | **string** |  | [optional] [default to undefined]
**endDate** | **string** |  | [optional] [default to undefined]
**order** | **number** |  | [optional] [default to undefined]
**sessions** | [**Array&lt;SessionDto&gt;**](SessionDto.md) |  | [optional] [default to undefined]
**links** | [**Array&lt;LinkDto&gt;**](LinkDto.md) |  | [optional] [default to undefined]
**flow** | [**FlowDto**](FlowDto.md) |  | [optional] [default to undefined]
**draft** | **boolean** |  | [optional] [default to undefined]

## Example

```typescript
import { ChapterDto } from './api';

const instance: ChapterDto = {
    heroes,
    places,
    id,
    title,
    description,
    story,
    startDate,
    endDate,
    order,
    sessions,
    links,
    flow,
    draft,
};
```

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)
