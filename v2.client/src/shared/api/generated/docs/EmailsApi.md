# EmailsApi

All URIs are relative to *http://localhost*

|Method | HTTP request | Description|
|------------- | ------------- | -------------|
|[**createEmail**](#createemail) | **POST** /Emails | |
|[**deleteEmailById**](#deleteemailbyid) | **DELETE** /Emails/{id} | |
|[**getEmail**](#getemail) | **GET** /Emails | |
|[**getEmailById**](#getemailbyid) | **GET** /Emails/{id} | |
|[**updateEmailById**](#updateemailbyid) | **PUT** /Emails/{id} | |
|[**updateEmailByIdSend**](#updateemailbyidsend) | **PUT** /Emails/{id}/send | |
|[**updateEmailByIdSendExternal**](#updateemailbyidsendexternal) | **PUT** /Emails/{id}/sendExternal | |

# **createEmail**
> createEmail()


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

const { status, data } = await apiInstance.createEmail(
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

# **deleteEmailById**
> deleteEmailById()


### Example

```typescript
import {
    EmailsApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new EmailsApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.deleteEmailById(
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

# **getEmail**
> Array<Email> getEmail()


### Example

```typescript
import {
    EmailsApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new EmailsApi(configuration);

let subject: string; // (optional) (default to undefined)
let sender: string; // (optional) (default to undefined)
let receiver: string; // (optional) (default to undefined)
let sentDateFrom: string; // (optional) (default to undefined)
let sentDateTo: string; // (optional) (default to undefined)

const { status, data } = await apiInstance.getEmail(
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

# **getEmailById**
> Email getEmailById()


### Example

```typescript
import {
    EmailsApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new EmailsApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.getEmailById(
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

# **updateEmailById**
> updateEmailById()


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

const { status, data } = await apiInstance.updateEmailById(
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

# **updateEmailByIdSend**
> updateEmailByIdSend()


### Example

```typescript
import {
    EmailsApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new EmailsApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.updateEmailByIdSend(
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

# **updateEmailByIdSendExternal**
> updateEmailByIdSendExternal()


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

const { status, data } = await apiInstance.updateEmailByIdSendExternal(
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

