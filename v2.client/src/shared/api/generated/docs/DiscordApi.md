# DiscordApi

All URIs are relative to *http://localhost*

|Method | HTTP request | Description|
|------------- | ------------- | -------------|
|[**get**](#get) | **GET** /api/Discord | |
|[**getById**](#getbyid) | **GET** /api/Discord/{id} | |
|[**updateById**](#updatebyid) | **PUT** /api/Discord/{id} | |

# **get**
> Array<DiscordCmd> get()


### Example

```typescript
import {
    DiscordApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new DiscordApi(configuration);

const { status, data } = await apiInstance.get();
```

### Parameters
This endpoint does not have any parameters.


### Return type

**Array<DiscordCmd>**

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **getById**
> Email getById()


### Example

```typescript
import {
    DiscordApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new DiscordApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.getById(
    id
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **id** | [**string**] |  | defaults to undefined|


### Return type

**Email**

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **updateById**
> updateById()


### Example

```typescript
import {
    DiscordApi,
    Configuration,
    DiscordCmd
} from './api';

const configuration = new Configuration();
const apiInstance = new DiscordApi(configuration);

let id: string; // (default to undefined)
let discordCmd: DiscordCmd; // (optional)

const { status, data } = await apiInstance.updateById(
    id,
    discordCmd
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **discordCmd** | **DiscordCmd**|  | |
| **id** | [**string**] |  | defaults to undefined|


### Return type

void (empty response body)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/*+json
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

