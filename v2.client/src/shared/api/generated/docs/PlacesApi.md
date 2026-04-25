# PlacesApi

All URIs are relative to *http://localhost*

|Method | HTTP request | Description|
|------------- | ------------- | -------------|
|[**createPlace**](#createplace) | **POST** /Places | |
|[**deletePlaceById**](#deleteplacebyid) | **DELETE** /Places/{id} | |
|[**getPlace**](#getplace) | **GET** /Places | |
|[**getPlaceById**](#getplacebyid) | **GET** /Places/{id} | |
|[**updatePlaceById**](#updateplacebyid) | **PUT** /Places/{id} | |

# **createPlace**
> createPlace()


### Example

```typescript
import {
    PlacesApi,
    Configuration,
    PlaceDto
} from './api';

const configuration = new Configuration();
const apiInstance = new PlacesApi(configuration);

let body: PlaceDto; // (optional)

const { status, data } = await apiInstance.createPlace(
    body
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **body** | **PlaceDto**|  | |


### Return type

void (empty response body)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/*+json
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **deletePlaceById**
> deletePlaceById()


### Example

```typescript
import {
    PlacesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new PlacesApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.deletePlaceById(
    id
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **id** | [**string**] |  | defaults to undefined|


### Return type

void (empty response body)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **getPlace**
> PlaceResponseList getPlace()


### Example

```typescript
import {
    PlacesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new PlacesApi(configuration);

let order: string; // (default to undefined)
let page: number; // (optional) (default to undefined)
let pageSize: number; // (optional) (default to undefined)
let orderBy: string; // (optional) (default to undefined)
let title: string; // (optional) (default to undefined)

const { status, data } = await apiInstance.getPlace(
    order,
    page,
    pageSize,
    orderBy,
    title
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **order** | [**string**] |  | defaults to undefined|
| **page** | [**number**] |  | (optional) defaults to undefined|
| **pageSize** | [**number**] |  | (optional) defaults to undefined|
| **orderBy** | [**string**] |  | (optional) defaults to undefined|
| **title** | [**string**] |  | (optional) defaults to undefined|


### Return type

**PlaceResponseList**

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **getPlaceById**
> Place getPlaceById()


### Example

```typescript
import {
    PlacesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new PlacesApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.getPlaceById(
    id
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **id** | [**string**] |  | defaults to undefined|


### Return type

**Place**

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **updatePlaceById**
> updatePlaceById()


### Example

```typescript
import {
    PlacesApi,
    Configuration,
    PlaceDto
} from './api';

const configuration = new Configuration();
const apiInstance = new PlacesApi(configuration);

let id: string; // (default to undefined)
let body: PlaceDto; // (optional)

const { status, data } = await apiInstance.updatePlaceById(
    id,
    body
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **body** | **PlaceDto**|  | |
| **id** | [**string**] |  | defaults to undefined|


### Return type

void (empty response body)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/*+json
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

