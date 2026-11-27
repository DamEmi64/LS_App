# SystemAutomateApi

All URIs are relative to *http://localhost*

|Method | HTTP request | Description|
|------------- | ------------- | -------------|
|[**get**](#get) | **GET** /api/SystemAutomate | |
|[**updateByIdActivate**](#updatebyidactivate) | **PUT** /api/SystemAutomate/{id}/activate | |
|[**updateByIdConfiguration**](#updatebyidconfiguration) | **PUT** /api/SystemAutomate/{id}/configuration | |
|[**updateByIdDeactivate**](#updatebyiddeactivate) | **PUT** /api/SystemAutomate/{id}/Deactivate | |

# **get**
> SystemAutomatResponseList get()


### Example

```typescript
import {
    SystemAutomateApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new SystemAutomateApi(configuration);

let order: string; // (default to undefined)
let page: number; // (optional) (default to undefined)
let pageSize: number; // (optional) (default to undefined)
let orderBy: string; // (optional) (default to undefined)

const { status, data } = await apiInstance.get(
    order,
    page,
    pageSize,
    orderBy
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **order** | [**string**] |  | defaults to undefined|
| **page** | [**number**] |  | (optional) defaults to undefined|
| **pageSize** | [**number**] |  | (optional) defaults to undefined|
| **orderBy** | [**string**] |  | (optional) defaults to undefined|


### Return type

**SystemAutomatResponseList**

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json, application/problem+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |
|**400** | Bad Request |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **updateByIdActivate**
> updateByIdActivate()


### Example

```typescript
import {
    SystemAutomateApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new SystemAutomateApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.updateByIdActivate(
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

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json, application/problem+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |
|**400** | Bad Request |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **updateByIdConfiguration**
> updateByIdConfiguration()


### Example

```typescript
import {
    SystemAutomateApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new SystemAutomateApi(configuration);

let id: string; // (default to undefined)
let body: string; // (optional)

const { status, data } = await apiInstance.updateByIdConfiguration(
    id,
    body
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **body** | **string**|  | |
| **id** | [**string**] |  | defaults to undefined|


### Return type

void (empty response body)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/*+json
 - **Accept**: application/json, application/problem+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |
|**400** | Bad Request |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **updateByIdDeactivate**
> updateByIdDeactivate()


### Example

```typescript
import {
    SystemAutomateApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new SystemAutomateApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.updateByIdDeactivate(
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

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json, application/problem+json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |
|**400** | Bad Request |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

