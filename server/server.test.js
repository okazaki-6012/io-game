const Server = require('./server')
const net = require('net')

let client

describe('server', () => {
  //new Server(3000);
  
  beforeEach(()=>{
    client = new net.Socket()
        client.setEncoding('utf8')
    client.connect('3000', 'localhost')
  })

  afterEach(()=>{
    client.destroy()
  })

  it('connection', done=>{
    client.on('data', data=>{
      expect(data).toBe('welcome!!')
      done()
    })
  })

  it('push message', done=>{
    client2 = new net.Socket()
    client2.setEncoding('utf8')
    client2.connect('3000', 'localhost', ()=>{
      // client2が接続完了後にclientでメッセージを送る
      client.write('hello!!')
    })
    client2.on('data', data=>{
      if(data !== 'welcome!!'){
        client2.destroy()
        expect(data).toBe('hello!!')
        done()
      }
    })
  })
})
