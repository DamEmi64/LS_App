# EmailsApi

All URIs are relative to *http://localhost*

|Method | HTTP request | Description|
|------------- | ------------- | -------------|
|[**create**](#create) | **POST** /api/Emails | |
|[**deleteById**](#deletebyid) | **DELETE** /api/Emails/{id} | |
|[**get**](#get) | **GET** /api/Emails | |
|[**getById**](#getbyid) | **GET** /api/Emails/{id} | |
|[**updateById**](#updatebyid) | **PUT** /api/Emails/{id} | |
|[**updateByIdSend**](#updatebyidsend) | **PUT** /api/Emails/{id}/send | |
|[**updateByIdSendExternal**](#updatebyidsendexternal) | **PUT** /api/Emails/{id}/sendExternal | |

# **create**
> create()


### Example

```typescript
import {
    EmailsApi,
    Configuration,
    Email
} from './api';

const configuration = new Configuration();
const apiInstance = new EmailsApi(configuration);

let body: Email; // (optional)

const { status, data } = await apiInstance.create(
    body
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **body** | **Email**|  | |


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

# **deleteById**
> deleteById()


### Example

```typescript
import {
    EmailsApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new EmailsApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.deleteById(
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

# **get**
> Array<Email> get()


### Example

```typescript
import {
    EmailsApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new EmailsApi(configuration);

let pageSize: number; // (optional) (default to undefined)
let page: number; // (optional) (default to undefined)
let subject: string; // (optional) (default to undefined)
let sender: string; // (optional) (default to undefined)
let receiver: string; // (optional) (default to undefined)
let sentDateFrom: string; // (optional) (default to undefined)
let sentDateTo: string; // (optional) (default to undefined)

const { status, data } = await apiInstance.get(
    pageSize,
    page,
    subject,
    sender,
    receiver,
    sentDateFrom,
    sentDateTo
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **pageSize** | [**number**] |  | (optional) defaults to undefined|
| **page** | [**number**] |  | (optional) defaults to undefined|
| **subject** | [**string**] |  | (optional) defaults to undefined|
| **sender** | [**string**] |  | (optional) defaults to undefined|
| **receiver** | [**string**] |  | (optional) defaults to undefined|
| **sentDateFrom** | [**string**] |  | (optional) defaults to undefined|
| **sentDateTo** | [**string**] |  | (optional) defaults to undefined|


### Return type

**Array<Email>**

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

# **getById**
> Email getById()


### Example

```typescript
import {
    EmailsApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new EmailsApi(configuration);

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

No authorization required

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
    EmailsApi,
    Configuration,
    Email
} from './api';

const configuration = new Configuration();
const apiInstance = new EmailsApi(configuration);

let id: string; // (default to undefined)
let body: Email; // (optional)

const { status, data } = await apiInstance.updateById(
    id,
    body
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **body** | **Email**|  | |
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

# **updateByIdSend**
> updateByIdSend()


### Example

```typescript
import {
    EmailsApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new EmailsApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.updateByIdSend(
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

# **updateByIdSendExternal**
> updateByIdSendExternal()


### Example

```typescript
import {
    EmailsApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new EmailsApi(configuration);

let id: string; // (default to undefined)
let sender: string; // (optional) (default to undefined)
let recipient: string; // (optional) (default to undefined)
let subject: string; // (optional) (default to undefined)
let body: string; // (optional)

const { status, data } = await apiInstance.updateByIdSendExternal(
    id,
    sender,
    recipient,
    subject,
    body
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **body** | **string**|  | |
| **id** | [**string**] |  | defaults to undefined|
| **sender** | [**string**] |  | (optional) defaults to undefined|
| **recipient** | [**string**] |  | (optional) defaults to undefined|
| **subject** | [**string**] |  | (optional) defaults to undefined|


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

